// -----------------------------------------------------------------------
// <copyright file="OmegaWarheadConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features;

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
        /// Gets or sets the cassie announcements to play leading up to the detonation.
        /// </summary>
        [Description("Cassie announcements to play leading up to the detonation. Starts immediately after alpha warhead detonation.")]
        public Dictionary<float, string> CassieAnnouncements { get; set; } = new()
        {
            { 37f, "Omega warhead detonation in T minus 30 seconds" },
        };

        /// <summary>
        /// Gets or sets the broadcasts to show leading up to the detonation.
        /// </summary>
        [Description("Broadcasts to show leading up to the detonation. Starts immediately after alpha warhead detonation.")]
        public Dictionary<float, Broadcast> Broadcasts { get; set; } = new()
        {
            { 37f, new Broadcast("Omega warhead detonation in T-30 seconds", 7) },
        };

        /// <summary>
        /// Gets or sets a value indicating whether the cassie noise should be suppressed.
        /// </summary>
        [Description("Whether the cassie noise should be suppressed.")]
        public bool SuppressCassieNoise { get; set; } = true;

        /// <summary>
        /// Gets or sets the time, in seconds, after the warhead detonates that the omega warhead should detonate.
        /// </summary>
        [Description("The time, in seconds, after the warhead detonates that the omega warhead should detonate.")]
        public float DetonationDelay { get; set; } = 80f;
    }
}