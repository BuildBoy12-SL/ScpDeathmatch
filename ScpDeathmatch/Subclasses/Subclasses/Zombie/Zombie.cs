// -----------------------------------------------------------------------
// <copyright file="Zombie.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Zombie
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using ScpDeathmatch.Models;

    /// <inheritdoc />
    public class Zombie : Subclass
    {
        private CoroutineHandle levelsCoroutine;
        private byte currentIntensity;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 300;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Zombie);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override ConfiguredBadge Badge { get; set; } = new(nameof(Zombie), "carmine");

        /// <summary>
        /// Gets or sets the times, in seconds, when Scp079s will be bumped a level.
        /// </summary>
        [Description("The times, in seconds, when Scp079s will be bumped a level.")]
        public List<float> ColaUpgradeTimes { get; set; } = new()
        {
            150f,
            300f,
            450f,
            600f,
        };

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            base.UnsubscribeEvents();
        }

        /// <inheritdoc />
        protected override void OnSpawned(SpawnedEventArgs ev)
        {
            if (Check(ev.Player))
                ev.Player.ChangeEffectIntensity<Scp207>(currentIntensity);

            base.OnSpawned(ev);
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (levelsCoroutine.IsRunning)
                Timing.KillCoroutines(levelsCoroutine);
        }

        private void OnRoundStarted()
        {
            currentIntensity = 1;
            if (levelsCoroutine.IsRunning)
                Timing.KillCoroutines(levelsCoroutine);

            levelsCoroutine = Timing.RunCoroutine(RunColaIntensityChecks());
        }

        private IEnumerator<float> RunColaIntensityChecks()
        {
            if (ColaUpgradeTimes is null)
                yield break;

            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);

                int nextIndex = ColaUpgradeTimes.Count - 1 >= currentIntensity ? currentIntensity : -1;
                if (nextIndex == -1)
                    break;

                if (Round.ElapsedTime.TotalSeconds < ColaUpgradeTimes[nextIndex])
                    continue;

                currentIntensity++;
                foreach (Player player in TrackedPlayers)
                {
                    if (player.IsAlive)
                        player.ChangeEffectIntensity<Scp207>(currentIntensity);
                }
            }
        }
    }
}