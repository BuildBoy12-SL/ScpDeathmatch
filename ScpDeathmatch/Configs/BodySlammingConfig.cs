// -----------------------------------------------------------------------
// <copyright file="BodySlammingConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using ScpDeathmatch.API.Interfaces;

    /// <summary>
    /// Handles configs related to body slamming.
    /// </summary>
    public class BodySlammingConfig : IConfigFile
    {
        /// <summary>
        /// Gets or sets the minimum amount of fall damage a player must take for it to be transferred.
        /// </summary>
        [Description("The minimum amount of fall damage a player must take for it to be transferred.")]
        public float MinimumDamage { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the farthest away a player can be from another player when they take damage for the damage to be transferred.
        /// </summary>
        [Description("The farthest away a player can be from another player when they take damage for the damage to be transferred.")]
        public float MaximumDistance { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the base multiplier for the damage taken when a player is body slammed.
        /// </summary>
        [Description("The base multiplier for the damage taken when a player is body slammed.")]
        public float DamageMultiplier { get; set; } = 2f;

        /// <summary>
        /// Gets or sets the adjusted multipliers for damage applied when the attacker is a subclass.
        /// </summary>
        [Description("The adjusted multipliers for damage applied when the attacker is a subclass.")]
        public Dictionary<string, float> ModifiedMultipliers { get; set; } = new()
        {
            { "Athlete", 2f },
            { "Brute", 2f },
            { "Insurgent", 2f },
            { "Marksman", 2f },
            { "Recon", 2f },
            { "Scavenger", 2f },
        };
    }
}