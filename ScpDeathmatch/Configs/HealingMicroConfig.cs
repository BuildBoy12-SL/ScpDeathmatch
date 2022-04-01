// -----------------------------------------------------------------------
// <copyright file="HealingMicroConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;

    /// <summary>
    /// Handles configs related to using the micro as a healing device.
    /// </summary>
    public class HealingMicroConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the healing micro is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the delay, in seconds, before health will be given.
        /// </summary>
        [Description("The initial delay, in seconds, before health will be given.")]
        public float InitialDelay { get; set; } = 2f;

        /// <summary>
        /// Gets or sets the amount of health per tick that should be healed.
        /// </summary>
        [Description("The amount of health per tick that should be healed.")]
        public float HealthPerTick { get; set; } = 2;

        /// <summary>
        /// Gets or sets the amount of time, in seconds, that should pass to be considered a tick.
        /// </summary>
        [Description("The amount of time, in seconds, that should pass to be considered a tick.")]
        public float SecondsPerTick { get; set; } = 0.2f;
    }
}