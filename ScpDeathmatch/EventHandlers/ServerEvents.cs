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
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Server"/>.
    /// </summary>
    public class ServerEvents
    {
        private readonly Plugin plugin;

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
            ServerHandlers.ReloadedConfigs += OnReloadedConfigs;
            ServerHandlers.RoundStarted += OnRoundStarted;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            ServerHandlers.EndingRound -= OnEndingRound;
            ServerHandlers.ReloadedConfigs -= OnReloadedConfigs;
            ServerHandlers.RoundStarted -= OnRoundStarted;
        }

        private void OnEndingRound(EndingRoundEventArgs ev)
        {
            ev.IsRoundEnded = Player.Get(player => player.IsAlive).Count() + plugin.RespawnManager.Count <= 1;
            ev.IsAllowed = true;
        }

        private void OnReloadedConfigs()
        {
            plugin.Config.Reload();
        }

        private void OnRoundStarted()
        {
            foreach (KeyValuePair<DoorType, float> kvp in plugin.Config.DoorLocks)
            {
                foreach (Door door in Door.Get(kvp.Key))
                    door.Lock(kvp.Value, DoorLockType.AdminCommand);
            }
        }
    }
}