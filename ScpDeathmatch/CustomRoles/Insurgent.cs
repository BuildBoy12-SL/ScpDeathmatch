// -----------------------------------------------------------------------
// <copyright file="Insurgent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class Insurgent : Subclass
    {
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
        /// Gets or sets the amount of time, in seconds, before this subclass can no longer respawn as an Scp079.
        /// </summary>
        [Description("The amount of time, in seconds, before this subclass can no longer respawn as an Scp079.")]
        public double RespawnCutoff { get; set; } = 450;

        /// <summary>
        /// Gets or sets the role for players to respawn as.
        /// </summary>
        public RoleType RespawnRole { get; set; } = RoleType.ChaosRifleman;

        /// <summary>
        /// Gets or sets a value indicating whether players of this subclass will be counted as alive when in Scp079 mode.
        /// </summary>
        public bool Count079Alive { get; set; } = false;

        /// <inheritdoc />
        public override List<string> Inventory { get; set; } = new()
        {
            "Jamming Coin",
        };

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; } = new();

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Scp079.GainingExperience += OnGainingExperience;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Scp079.GainingExperience -= OnGainingExperience;
            base.UnsubscribeEvents();
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (Check(ev.Target) && Round.ElapsedTime.TotalSeconds < RespawnCutoff && !Warhead.IsDetonated && !Recontainer.Base._alreadyRecontained)
                ev.Target.Role.Type = RoleType.Scp079;
        }

        private void OnGainingExperience(GainingExperienceEventArgs ev)
        {
            if (!Check(ev.Player))
                return;

            ev.IsAllowed = false;
            if (ev.GainType is ExpGainType.DirectKill or ExpGainType.KillAssist or ExpGainType.PocketAssist)
                ev.Player.Role.Type = RespawnRole;
        }
    }
}