// -----------------------------------------------------------------------
// <copyright file="StatusAffliction.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Recon.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using ScpDeathmatch.Models;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class StatusAffliction : ActiveAbility
    {
        /// <inheritdoc />
        public override string Name { get; set; } = "Status Affliction";

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        [YamlIgnore]
        public override float Duration { get; set; }

        /// <inheritdoc />
        public override float Cooldown { get; set; } = 30f;

        /// <summary>
        /// Gets or sets the effects to apply to the user.
        /// </summary>
        [Description("The effects to apply to the user.")]
        public List<ConfiguredEffect> SelfEffects { get; set; } = new()
        {
            new ConfiguredEffect(EffectType.Vitality, 5, 5f),
            new ConfiguredEffect(EffectType.Invigorated, 5, 5f),
        };

        /// <summary>
        /// Gets or sets the effects to apply to all other players.
        /// </summary>
        [Description("The effects to apply to all other players.")]
        public List<ConfiguredEffect> TargetEffects { get; set; } = new()
        {
            new ConfiguredEffect(EffectType.Exhausted, 5, 5f),
        };

        /// <inheritdoc />
        protected override void AbilityUsed(Player player)
        {
            foreach (ConfiguredEffect effect in SelfEffects)
                effect.Apply(player);

            foreach (Player target in Player.List)
            {
                if (target == player)
                    continue;

                foreach (ConfiguredEffect effect in TargetEffects)
                    effect.Apply(target);
            }
        }
    }
}