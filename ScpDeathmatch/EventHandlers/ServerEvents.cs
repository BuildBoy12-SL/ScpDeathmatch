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
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (ConfiguredCommand command in plugin.Config.Commands.RoundEnd)
                coroutineHandles.Add(command.Execute());
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