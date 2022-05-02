// -----------------------------------------------------------------------
// <copyright file="SpeedyMicroConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;

    /// <summary>
    /// Handles configs related to using the micro as a movement enhancement device.
    /// </summary>
    public class SpeedyMicroConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the speedy micro is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the delay, in seconds, before speed will be given.
        /// </summary>
        [Description("The initial delay, in seconds, before speed will be given.")]
        public float InitialDelay { get; set; } = 2f;

        /// <summary>
        /// Gets or sets the amount of time, in seconds, that should pass to be considered a tick.
        /// </summary>
        [Description("The amount of time, in seconds, that should pass to be considered a tick.")]
        public float SecondsPerTick { get; set; } = 0.2f;

        /// <summary>
        /// Gets or sets the maximum boost intensity.
        /// </summary>
        [Description("The maximum boost intensity.")]
        public byte MaximumBoost { get; set; } = 50;

        /// <summary>
        /// Gets or sets the amount of movement boost to increase per tick while micro is active.
        /// </summary>
        [Description("The amount of movement boost to increase per tick while micro is active.")]
        public byte BoostIncreasePerTick { get; set; } = 5;

        /// <summary>
        /// Gets or sets the amount of movement boost to decrease per tick after the micro is no longer in use.
        /// </summary>
        [Description("The amount of movement boost to decrease per tick after the micro is no longer in use.")]
        public byte BoostDecreasePerTick { get; set; } = 5;
    }
}