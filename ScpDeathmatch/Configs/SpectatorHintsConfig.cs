// -----------------------------------------------------------------------
// <copyright file="SpectatorHintsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Handles configs related to showing spectators hints.
    /// </summary>
    public class SpectatorHintsConfig
    {
        /// <summary>
        /// Gets or sets the amount of time, in seconds, between displaying a hint.
        /// </summary>
        [Description("The amount of time, in seconds, between displaying a hint.")]
        public float Interval { get; set; } = 45f;

        /// <summary>
        /// Gets or sets the time, in seconds, to display the hint.
        /// </summary>
        [Description("The time, in seconds, to display the hint.")]
        public float DisplayDuration { get; set; } = 7f;

        /// <summary>
        /// Gets or sets the hints to choose from.
        /// </summary>
        [Description("The hints to choose from.")]
        public List<string> AvailableHints { get; set; } = new()
        {
            "<color=yellow>Scavengers</color> have a passive ability that allows them to automatically pick up items",
            "You can use medical items instantly as the <color=red>Nurse</color> subclass",
        };
    }
}