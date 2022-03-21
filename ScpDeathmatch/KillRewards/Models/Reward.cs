// -----------------------------------------------------------------------
// <copyright file="Reward.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.KillRewards.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features;

    /// <summary>
    /// Handles configs related to granting rewards for specific actions.
    /// </summary>
    public class Reward
    {
        /// <summary>
        /// Gets or sets the broadcast to send when someone performs a kill with a headshot.
        /// </summary>
        [Description("The broadcast to send when someone performs the reward action. Variables: %killer, %victim")]
        public Broadcast Broadcast { get; set; } = new Broadcast("<color=red>%killer just big ironed %victim!</color>", 5);

        /// <summary>
        /// Gets or sets the amount of ahp to reward a player with.
        /// </summary>
        [Description("The amount of ahp to reward a player with.")]
        public int AhpAmount { get; set; } = 20;

        /// <summary>
        /// Gets or sets the amount of hp to regen per second to reward a player with.
        /// </summary>
        [Description("The amount of hp to regen per second to reward a player with.")]
        public int HpRegenAmount { get; set; } = 30;

        /// <summary>
        /// Gets or sets the amount of time of regeneration to reward a player with.
        /// </summary>
        [Description("The amount of time of regeneration to reward a player with.")]
        public float HpRegenDuration { get; set; } = 5f;

        /// <summary>
        /// Gets or sets the effects to reward a player with.
        /// </summary>
        [Description("The effects to reward a player with.")]
        public List<ConfiguredEffect> Effects { get; set; } = new List<ConfiguredEffect>
        {
            new ConfiguredEffect
            {
                Type = EffectType.MovementBoost,
                Duration = 15f,
                Intensity = 1,
            },
            new ConfiguredEffect
            {
                Type = EffectType.DamageReduction,
                Duration = 15f,
                Intensity = 1,
            },
        };
    }
}