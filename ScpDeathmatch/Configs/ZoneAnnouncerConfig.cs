// -----------------------------------------------------------------------
// <copyright file="ZoneAnnouncerConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using ScpDeathmatch.Managers;

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using ScpDeathmatch.Commands;

    /// <summary>
    /// Handles configs for the <see cref="ZoneAnnouncer"/>.
    /// </summary>
    public class ZoneAnnouncerConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the announcer is enabled.
        /// </summary>
        [Description("Whether the announcer is enabled.")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the initial delay of the announcer.
        /// </summary>
        public float AnnouncerFirstDelay { get; set; } = 120f;

        /// <summary>
        /// Gets or sets the delay between each announcement.
        /// </summary>
        public float AnnouncerDelay { get; set; } = 60f;

        /// <summary>
        /// Gets or sets the message to append to the start of the zone announcement.
        /// </summary>
        public string StartupNoise { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the announcements for each zone.
        /// </summary>
        public Dictionary<ZoneType, string> Announcements { get; set; } = new Dictionary<ZoneType, string>
        {
            [ZoneType.Entrance] = string.Empty,
            [ZoneType.HeavyContainment] = string.Empty,
            [ZoneType.LightContainment] = string.Empty,
            [ZoneType.Surface] = string.Empty,
            [ZoneType.Unspecified] = string.Empty,
        };

        /// <summary>
        /// Gets or sets a value indicating whether the cassie noise will be suppressed.
        /// </summary>
        public bool SuppressCassieNoise { get; set; } = true;

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.ForceAnnouncerCommand"/> class.
        /// </summary>
        public ForceAnnouncerCommand ForceAnnouncerCommand { get; set; } = new ForceAnnouncerCommand();
    }
}