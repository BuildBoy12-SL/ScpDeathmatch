// -----------------------------------------------------------------------
// <copyright file="Insurgent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.CustomRoles.API.Features;
    using MEC;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomRole(RoleType.ClassD)]
    public class Insurgent : Subclass
    {
        private CoroutineHandle respawnCoroutine;

        /// <inheritdoc />
        public override uint Id { get; set; } = 102;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Insurgent);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override string Badge { get; set; } = nameof(Insurgent);

        /// <inheritdoc />
        public override string BadgeColor { get; set; } = "emerald";

        /// <inheritdoc />
        public override string DeadBadge { get; set; } = "Dead Insurgent";

        /// <inheritdoc />
        public override string DeadBadgeColor { get; set; } = "nickel";

        /// <summary>
        /// Gets or sets the amount of time, in seconds, before this subclass can no longer respawn.
        /// </summary>
        public double RespawnCutoff { get; set; } = 450;

        /// <summary>
        /// Gets or sets the role for players to respawn as.
        /// </summary>
        public RoleType RespawnRole { get; set; } = RoleType.ChaosRifleman;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; } = new();

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            base.UnsubscribeEvents();
        }

        private void OnRoundStarted()
        {
            if (respawnCoroutine.IsRunning)
                Timing.KillCoroutines(respawnCoroutine);

            respawnCoroutine = Timing.RunCoroutine(RunRespawn());
        }

        private IEnumerator<float> RunRespawn()
        {
            while (Round.ElapsedTime.TotalSeconds < RespawnCutoff)
                yield return Timing.WaitForSeconds(1f);

            foreach (Player player in TrackedPlayers)
            {
                if (player.IsDead)
                    player.Role.Type = RespawnRole;
            }
        }
    }
}