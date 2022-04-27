// -----------------------------------------------------------------------
// <copyright file="RoundStatsManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    /// <summary>
    /// Handles the counting of kills and display of stats at the end of the round.
    /// </summary>
    public class RoundStatsManager
    {
        private readonly Plugin plugin;
        private readonly SortedList<Player, int> kills = new();
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
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (!Round.IsStarted || ev.Killer is null || ev.Killer == ev.Target)
                return;

            if (firstBlood is null)
                firstBlood = ev.Killer;

            if (!kills.ContainsKey(ev.Killer))
                kills.Add(ev.Killer, 0);

            kills[ev.Killer]++;
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Broadcast broadcast = plugin.Config.StatBroadcast.Broadcast;
            string content = FormatBroadcast();
            if (broadcast.Show && !string.IsNullOrEmpty(content))
                Map.Broadcast(broadcast.Duration, content, broadcast.Type, true);
        }

        private void OnWaitingForPlayers()
        {
            kills.Clear();
            firstBlood = null;
        }

        private string FormatBroadcast()
        {
            Player winner = Player.Get(player => player.IsAlive && !(plugin.Config.Subclasses.Insurgent.Check(player) && player.Role.Type == RoleType.Scp079)).FirstOrDefault();
            string winnerName = winner?.DisplayNickname ?? winner?.Nickname;
            string firstBloodName = firstBlood?.DisplayNickname ?? firstBlood?.Nickname;

            int topKillCount = 0;
            string topKillName = null;
            if (kills.Count > 0)
            {
                KeyValuePair<Player, int> topKills = kills.First();
                topKillName = topKills.Key?.DisplayNickname ?? topKills.Key?.Nickname;
                topKillCount = topKills.Value;
            }

            return plugin.Config.StatBroadcast.Broadcast.Content
                .Replace("$Winner", string.IsNullOrEmpty(winnerName) ? string.Empty : string.Format(plugin.Config.StatBroadcast.Winner, winnerName))
                .Replace("$TopKills", string.IsNullOrEmpty(topKillName) || topKillCount == 0 ? string.Empty : string.Format(plugin.Config.StatBroadcast.TopKills, topKillName, topKillCount))
                .Replace("$FirstBlood", string.IsNullOrEmpty(firstBloodName) ? string.Empty : string.Format(plugin.Config.StatBroadcast.FirstBlood, firstBloodName));
        }
    }
}