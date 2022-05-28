// -----------------------------------------------------------------------
// <copyright file="OmegaWarhead.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using MEC;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles the running of the omega warhead.
    /// </summary>
    public class OmegaWarhead : Subscribable
    {
        private readonly List<CoroutineHandle> displayCoroutines = new();
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
            KillDisplayCoroutines();
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
            foreach (KeyValuePair<float, string> kvp in Plugin.Config.OmegaWarhead.CassieAnnouncements)
            {
                displayCoroutines.Add(Timing.CallDelayed(kvp.Key, () =>
                {
                    Cassie.Message(kvp.Value, isNoisy: !Plugin.Config.OmegaWarhead.SuppressCassieNoise);
                }));
            }

            foreach (KeyValuePair<float, Broadcast> kvp in Plugin.Config.OmegaWarhead.Broadcasts)
            {
                displayCoroutines.Add(Timing.CallDelayed(kvp.Key, () =>
                {
                    Map.Broadcast(kvp.Value);
                }));
            }

            yield return Timing.WaitForSeconds(Plugin.Config.OmegaWarhead.DetonationDelay);
            Warhead.Detonate();
            KillDisplayCoroutines();
            foreach (Player player in Player.List)
            {
                if (!player.SessionVariables.ContainsKey("IsNPC") && !player.IsGodModeEnabled)
                    player.Kill("Vaporized by the Omega Warhead.");
            }
        }

        private void KillDisplayCoroutines()
        {
            foreach (CoroutineHandle coroutineHandle in displayCoroutines)
            {
                if (coroutineHandle.IsRunning)
                    Timing.KillCoroutines(coroutineHandle);
            }

            displayCoroutines.Clear();
        }
    }
}