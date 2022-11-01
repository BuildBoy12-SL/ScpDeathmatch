// -----------------------------------------------------------------------
// <copyright file="RewardsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using ScpDeathmatch.API.Interfaces;
    using ScpDeathmatch.KillRewards;
    using ScpDeathmatch.KillRewards.Models;

    /// <summary>
    /// Handles configs for the <see cref="RewardManager"/>.
    /// </summary>
    public class RewardsConfig : IConfigFile
    {
        /// <summary>
        /// Gets or sets a value indicating whether the rewards system is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a collection of reward configs.
        /// </summary>
        [Description("A collection of reward configs.")]
        public List<RewardRequirement> Rewards { get; set; } = new()
        {
            new RewardRequirement
            {
                DamageType = DamageType.Revolver,
                SpecifyHitbox = true,
                Hitbox = HitboxType.Headshot,
                Config = new Reward(),
            },
        };
    }
}