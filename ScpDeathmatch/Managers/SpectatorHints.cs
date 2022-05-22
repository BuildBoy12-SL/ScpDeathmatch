// -----------------------------------------------------------------------
// <copyright file="SpectatorHints.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Collections.Generic;
    using AdvancedHints;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using ScpDeathmatch.API.Extensions;
    using ScpDeathmatch.Models;

    /// <inheritdoc />
    public class SpectatorHints : Subscribable
    {
        private CoroutineHandle coroutineHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectatorHints"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public SpectatorHints(ScpDeathmatch.Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnRoundStarted()
        {
            if (coroutineHandle.IsRunning)
                Timing.KillCoroutines(coroutineHandle);

            coroutineHandle = Timing.RunCoroutine(RunHints());
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (coroutineHandle.IsRunning)
                Timing.KillCoroutines(coroutineHandle);
        }

        private void OnWaitingForPlayers()
        {
            if (coroutineHandle.IsRunning)
                Timing.KillCoroutines(coroutineHandle);
        }

        private IEnumerator<float> RunHints()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(Plugin.Config.SpectatorHints.Interval);
                foreach (Player player in Player.List)
                {
                    if (player.IsAlive)
                        continue;

                    string randomHint = Plugin.Config.SpectatorHints.AvailableHints.Random();
                    if (randomHint is not null)
                        player.ShowManagedHint(randomHint, Plugin.Config.SpectatorHints.Duration, true, Plugin.Config.SpectatorHints.Location);
                }
            }
        }
    }
}