using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using ScpDeathmatch.EventHandlers;

namespace ScpDeathmatch.Managers
{
    public class RoundStatsManager
    {
        private readonly Plugin plugin;
        private readonly Dictionary<Player, int> kills = new Dictionary<Player, int>();
        private Player firstBlood;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundStatsManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public RoundStatsManager(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (ev.Killer == null || ev.Killer == ev.Target)
                return;

            if (firstBlood == null)
                firstBlood = ev.Killer;

            if (!kills.ContainsKey(ev.Killer))
                kills.Add(ev.Killer, 0);

            kills[ev.Killer]++;
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            List<KeyValuePair<Player, int>> sortedDictionary = kills.OrderBy(entry => entry.Value).ToList();
            KeyValuePair<Player, int> topKills = sortedDictionary.FirstOrDefault();
            string topKillsName = sortedDictionary.Count == 0 ? "Unknown" : topKills.Key.DisplayNickname ?? topKills.Key.Nickname;
            int topKillsAmount = sortedDictionary.Count == 0 ? 0 : topKills.Value;

            Player winner = Player.Get(player => player.IsAlive).FirstOrDefault();
            string winnerName = winner == null ? "Unknown" : winner.DisplayNickname ?? winner.Nickname;

            Exiled.API.Features.Broadcast broadcast = plugin.Config.RoundEndBroadcast;
            string content = broadcast.Content
                .Replace("$TopKillsAmount", topKillsAmount.ToString())
                .Replace("$TopKills", topKillsName)
                .Replace("$Winner", winnerName ?? "Unknown")
                .Replace("$FirstBlood", firstBlood == null ? "Unknown" : firstBlood.DisplayNickname ?? firstBlood.Nickname);

            if (broadcast.Show)
                Map.Broadcast(broadcast.Duration, content, broadcast.Type, true);

            kills.Clear();
            firstBlood = null;
        }
    }
}