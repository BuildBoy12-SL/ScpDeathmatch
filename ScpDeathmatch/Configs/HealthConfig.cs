// -----------------------------------------------------------------------
// <copyright file="HealthConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;
    using ScpDeathmatch.API.Interfaces;
    using UnityEngine;

    /// <summary>
    /// Handles configs related to health.
    /// </summary>
    public class HealthConfig : IConfigFile
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
        /// Gets or sets a value indicating whether the cooldown will be bypassed while the player has the invigorated effect.
        /// </summary>
        [Description("Whether the cooldown will be bypassed while the player has the invigorated effect.")]
        public bool InvigoratedBypassDelay { get; set; } = false;

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

        /// <summary>
        /// Gets or sets a value indicating whether Scp1853's stamina penalty will be ignored.
        /// </summary>
        [Description("Whether Scp1853's stamina penalty will be ignored.")]
        public bool Scp1853StaminaImmune { get; set; } = true;

        /// <summary>
        /// Gets or sets the amount of stamina to add when they hit the stamina threshold.
        /// </summary>
        [Description("The amount of stamina to add when they hit the stamina threshold.")]
        public float LfsStaminaAdded { get; set; } = 0.05f;

        /// <summary>
        /// Gets or sets the amount of maximum health to be removed from a player when they get additional stamina from LFS.
        /// </summary>
        [Description("The amount of maximum health to be removed from a player when they get additional stamina from LFS.")]
        public int LfsHpRemoved { get; set; } = 1;
    }
}