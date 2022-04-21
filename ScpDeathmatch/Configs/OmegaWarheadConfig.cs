﻿// -----------------------------------------------------------------------
// <copyright file="OmegaWarheadConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;

    /// <summary>
    /// Handles configs related to the Omega Warhead.
    /// </summary>
    public class OmegaWarheadConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the omega warhead is enabled.
        /// </summary>
        [Description("Whether the omega warhead is enabled.")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the cassie announcement to play when the warhead starts.
        /// </summary>
        [Description("The cassie announcement to play when the warhead starts.")]
        public string Cassie { get; set; } = "omega warhead detonation in 30 . 29 . 28 . 27 . 26 . 25 . 24 . 23 . 22 . 21 . 20 . 19 . 18 . 17 . 16 . 15 . 14 . 13 . 12 . 11 . 10 seconds . 9 . 8 . 7 . 6 . 5 . 4 . 3 . 2 . 1 .";

        /// <summary>
        /// Gets or sets a value indicating whether the cassie noise should be suppressed.
        /// </summary>
        [Description("Whether the cassie noise should be suppressed.")]
        public bool SuppressCassieNoise { get; set; } = true;

        /// <summary>
        /// Gets or sets the time, in seconds, between the alpha warhead detonating and the omega warhead sequence starting.
        /// </summary>
        [Description("The time, in seconds, between the alpha warhead detonating and the omega warhead sequence starting.")]
        public float InitialDelay { get; set; } = 37f;

        /// <summary>
        /// Gets or sets the time, in seconds, after the warhead detonates that the omega warhead should detonate.
        /// </summary>
        [Description("The time, in seconds, after the warhead detonates that the omega warhead should detonate.")]
        public float Time { get; set; } = 43f;
    }
}