// -----------------------------------------------------------------------
// <copyright file="OmegaWarhead.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using MEC;

    /// <summary>
    /// Handles the running of the omega warhead.
    /// </summary>
    public class OmegaWarhead
    {
        private readonly Plugin plugin;
        private CoroutineHandle warheadCoroutine;
        private bool isOmega;

        /// <summary>
        /// Initializes a new instance of the <see cref="OmegaWarhead"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public OmegaWarhead(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Warhead.Detonated += OnDetonated;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Warhead.Detonated -= OnDetonated;
        }

        private void OnWaitingForPlayers()
        {
            isOmega = false;
            if (warheadCoroutine.IsRunning)
                Timing.KillCoroutines(warheadCoroutine);
        }

        private void OnDetonated()
        {
            if (isOmega)
                return;

            warheadCoroutine = Timing.RunCoroutine(RunWarhead());
            isOmega = true;
        }

        private IEnumerator<float> RunWarhead()
        {
            yield return Timing.WaitForSeconds(plugin.Config.OmegaWarhead.InitialDelay);
            Cassie.Message(plugin.Config.OmegaWarhead.Cassie, isNoisy: !plugin.Config.OmegaWarhead.SuppressCassieNoise);
            yield return Timing.WaitForSeconds(plugin.Config.OmegaWarhead.Time);
            AlphaWarheadController.Host.InstantPrepare();
            AlphaWarheadController.Host.StartDetonation();
            AlphaWarheadController.Host.NetworktimeToDetonation = 0.1f;
            foreach (Player player in Player.List)
                player.Kill(DamageType.Warhead);
        }
    }
}