﻿// -----------------------------------------------------------------------
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
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles the running of the omega warhead.
    /// </summary>
    public class OmegaWarhead : Subscribable
    {
        private CoroutineHandle warheadCoroutine;
        private bool isOmega;

        /// <summary>
        /// Initializes a new instance of the <see cref="OmegaWarhead"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public OmegaWarhead(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Warhead.Detonated += OnDetonated;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
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
            if (!Plugin.Config.OmegaWarhead.IsEnabled || isOmega)
                return;

            warheadCoroutine = Timing.RunCoroutine(RunWarhead());
            isOmega = true;
        }

        private IEnumerator<float> RunWarhead()
        {
            yield return Timing.WaitForSeconds(Plugin.Config.OmegaWarhead.InitialDelay);
            Cassie.Message(Plugin.Config.OmegaWarhead.Cassie, isNoisy: !Plugin.Config.OmegaWarhead.SuppressCassieNoise);
            yield return Timing.WaitForSeconds(Plugin.Config.OmegaWarhead.Time);
            AlphaWarheadController.Host.InstantPrepare();
            AlphaWarheadController.Host.StartDetonation();
            AlphaWarheadController.Host.NetworktimeToDetonation = 0.1f;
            foreach (Player player in Player.List)
            {
                if (player.SessionVariables.ContainsKey("IsNPC"))
                    continue;

                player.Kill(DamageType.Warhead);
            }
        }
    }
}