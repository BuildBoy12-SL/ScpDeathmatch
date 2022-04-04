// -----------------------------------------------------------------------
// <copyright file="Runner.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.CustomRoles.API.Features;
    using GameCore;
    using MEC;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomRole(RoleType.ClassD)]
    public class Runner : Subclass
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 105;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Runner);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override string Badge { get; set; } = nameof(Runner);

        /// <inheritdoc />
        public override string BadgeColor { get; set; } = "yellow";

        /// <summary>
        /// Gets or sets the multiplier for movement speed.
        /// </summary>
        [Description("The multiplier for movement speed.")]
        public float MovementMultiplier { get; set; } = 1.15f;

        /// <summary>
        /// Gets or sets the multiplier for stamina capacity.
        /// </summary>
        [Description("The multiplier for stamina capacity.")]
        public float StaminaMultiplier { get; set; } = 1.2f;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>();

        /// <inheritdoc />
        protected override void RoleAdded(Player player)
        {
            player.Stamina.StaminaUse /= StaminaMultiplier;
            Timing.CallDelayed(1.5f, () =>
            {
                player.ChangeWalkingSpeed(MovementMultiplier);
                player.ChangeRunningSpeed(MovementMultiplier);
            });

            base.RoleAdded(player);
        }

        /// <inheritdoc />
        protected override void RoleRemoved(Player player)
        {
            player.Stamina.StaminaUse = ConfigFile.ServerConfig.GetFloat("stamina_balance_use", 0.05f);
            Timing.CallDelayed(1.5f, () =>
            {
                player.ChangeWalkingSpeed(1f);
                player.ChangeRunningSpeed(1f);
            });

            base.RoleRemoved(player);
        }
    }
}