// -----------------------------------------------------------------------
// <copyright file="HealthConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;
    using UnityEngine;

    /// <summary>
    /// Handles configs related to health.
    /// </summary>
    public class HealthConfig
    {
        private int maxHealthPercentage = 20;
        private int regenPercentage = 10;

        /// <summary>
        /// Gets or sets the percentage of the damage that is dealt to reduce from the player's maximum health.
        /// </summary>
        [Description("The percentage of the damage that is dealt to reduce from the player's maximum health.")]
        public int MaxHealthPercentage
        {
            get => maxHealthPercentage;
            set => maxHealthPercentage = Mathf.Clamp(value, 0, 100);
        }

        /// <summary>
        /// Gets or sets the delay, in seconds, after a player is hurt that they will being regenerating.
        /// </summary>
        [Description("The delay, in seconds, after a player is hurt that they will being regenerating.")]
        public float RegenDelay { get; set; } = 30f;

        /// <summary>
        /// Gets or sets the amount of time, in seconds, between each tick while regenerating.
        /// </summary>
        [Description("The amount of time, in seconds, between each tick while regenerating.")]
        public float RegenerationTick { get; set; } = 0.2f;

        /// <summary>
        /// Gets or sets the percentage of the damage that is dealt to reduce from the player's maximum health.
        /// </summary>
        [Description("The percentage of the players maximum health to add to the player every tick while regenerating.")]
        public int RegenPercentage
        {
            get => regenPercentage;
            set => regenPercentage = Mathf.Clamp(value, 0, 100);
        }

        /// <summary>
        /// Gets or sets a value indicating whether Scp207 will deal damage.
        /// </summary>
        [Description("Whether Scp207 will deal damage.")]
        public bool Scp207Damage { get; set; } = false;
    }
}