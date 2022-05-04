// -----------------------------------------------------------------------
// <copyright file="ServerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.EventHandlers
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using ScpDeathmatch.Models;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Server"/>.
    /// </summary>
    public class ServerEvents : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerEvents"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public ServerEvents(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            ServerHandlers.ReloadedConfigs += OnReloadedConfigs;
            ServerHandlers.RoundStarted += OnRoundStarted;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            ServerHandlers.ReloadedConfigs -= OnReloadedConfigs;
            ServerHandlers.RoundStarted -= OnRoundStarted;
        }

        private void OnReloadedConfigs()
        {
            Plugin.Config.Reload();
        }

        private void OnRoundStarted()
        {
            foreach (KeyValuePair<DoorType, float> kvp in Plugin.Config.DoorLocks)
                Door.Get(kvp.Key)?.Lock(kvp.Value, DoorLockType.AdminCommand);

            foreach (Generator generator in Generator.List)
                generator.ActivationTime = Plugin.Config.DefaultGeneratorTime;
        }
    }
}