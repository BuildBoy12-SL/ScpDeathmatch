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
        /// Gets or sets the zones that the announcer will skip.
        /// </summary>
        public List<ZoneType> DisabledZones { get; set; } = new List<ZoneType>
        {
            ZoneType.Unspecified,
        };

        /// <summary>
        /// Gets or sets the initial delay of the announcer.
        /// </summary>
        public float AnnouncerFirstDelay { get; set; } = 120f;

        /// <summary>
        /// Gets or sets the delay between each announcement.
        /// </summary>
        public float AnnouncerDelay { get; set; } = 60f;

        /// <summary>
        /// Gets or sets the announcement for players in <see cref="ZoneType.Entrance"/>.
        /// </summary>
        public string EntranceAnnouncement { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the announcement for players in <see cref="ZoneType.HeavyContainment"/>.
        /// </summary>
        public string HeavyContainmentAnnouncement { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the announcement for players in <see cref="ZoneType.LightContainment"/>.
        /// </summary>
        public string LightContainmentAnnouncement { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the announcement for players in <see cref="ZoneType.Surface"/>.
        /// </summary>
        public string SurfaceAnnouncement { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the announcement for players in <see cref="ZoneType.Unspecified"/>.
        /// </summary>
        public string UnspecifiedAnnouncement { get; set; } = string.Empty;

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