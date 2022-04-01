// -----------------------------------------------------------------------
// <copyright file="ServerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.EventHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using LightContainmentZoneDecontamination;
    using MEC;
    using ScpDeathmatch.Models;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Server"/>.
    /// </summary>
    public class ServerEvents
    {
        private readonly Plugin plugin;
        private readonly List<CoroutineHandle> coroutineHandles = new List<CoroutineHandle>();
        private string winnerName = "Unknown";

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerEvents"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public ServerEvents(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            ServerHandlers.EndingRound += OnEndingRound;
            ServerHandlers.RoundEnded += OnRoundEnded;
            ServerHandlers.RoundStarted += OnRoundStarted;
            ServerHandlers.WaitingForPlayers += OnWaitingForPlayers;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            ServerHandlers.EndingRound -= OnEndingRound;
            ServerHandlers.RoundEnded -= OnRoundEnded;
            ServerHandlers.RoundStarted -= OnRoundStarted;
            ServerHandlers.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnEndingRound(EndingRoundEventArgs ev)
        {
            ev.IsAllowed = Player.Get(player => player.IsAlive).Count() + plugin.RespawnManager.Count <= 1;
            ev.IsRoundEnded = true;
            winnerName = Player.Get(player => player.IsAlive).FirstOrDefault()?.Nickname ?? "Unknown";
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (ConfiguredCommand command in plugin.Config.Commands.RoundEnd)
                coroutineHandles.Add(command.Execute());

            List<KeyValuePair<Player, int>> sortedDictionary = plugin.PlayerEvents.Kills.OrderBy(entry => entry.Value).ToList();
            KeyValuePair<Player, int> topKills = sortedDictionary.FirstOrDefault();
            string topKillsName = sortedDictionary.Count == 0 ? "Unknown" : topKills.Key.DisplayNickname ?? topKills.Key.Nickname;
            int topKillsAmount = sortedDictionary.Count == 0 ? 0 : topKills.Value;

            Broadcast broadcast = plugin.Config.RoundEndBroadcast;
            string content = broadcast.Content
                .Replace("$TopKillsAmount", topKillsAmount.ToString())
                .Replace("$TopKills", topKillsName)
                .Replace("$Winner", winnerName ?? "Unknown")
                .Replace("$FirstBlood", plugin.PlayerEvents.FirstBlood == null ? "Unknown" : plugin.PlayerEvents.FirstBlood.DisplayNickname ?? plugin.PlayerEvents.FirstBlood.Nickname);

            if (broadcast.Show)
                Map.Broadcast(broadcast.Duration, content, broadcast.Type, true);

            plugin.PlayerEvents.Kills.Clear();
            plugin.PlayerEvents.FirstBlood = null;
        }

        private void OnRoundStarted()
        {
            foreach (ConfiguredCommand command in plugin.Config.Commands.RoundStart)
                coroutineHandles.Add(command.Execute());

            foreach (KeyValuePair<DoorType, float> kvp in plugin.Config.DoorLocks)
            {
                foreach (Door door in Door.Get(kvp.Key))
                    door.Lock(kvp.Value, DoorLockType.AdminCommand);
            }
        }

        private void OnWaitingForPlayers()
        {
            if (plugin.Config.Decontamination.IsEnabled)
            {
                DecontaminationController.Singleton.NetworkRoundStartTime = -1.0;
                DecontaminationController.Singleton._stopUpdating = true;
            }

            foreach (CoroutineHandle coroutineHandle in coroutineHandles)
            {
                if (coroutineHandle.IsRunning)
                    Timing.KillCoroutines(coroutineHandle);
            }

            coroutineHandles.Clear();
        }
    }
}