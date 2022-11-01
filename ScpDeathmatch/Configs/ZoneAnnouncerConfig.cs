// -----------------------------------------------------------------------
// <copyright file="ZoneAnnouncerConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using ScpDeathmatch.API.Interfaces;
    using ScpDeathmatch.Commands;
    using ScpDeathmatch.Managers;

    /// <summary>
    /// Handles configs for the <see cref="ZoneAnnouncer"/>.
    /// </summary>
    public class ZoneAnnouncerConfig : IConfigFile
    {
        /// <summary>
        /// Gets or sets a value indicating whether the announcer is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the initial delay of the announcer.
        /// </summary>
        [Description("The initial delay of the announcer.")]
        public float AnnouncerFirstDelay { get; set; } = 120f;

        /// <summary>
        /// Gets or sets the delay between each announcement.
        /// </summary>
        [Description("The delay between each announcement.")]
        public float AnnouncerDelay { get; set; } = 60f;

        /// <summary>
        /// Gets or sets the message to append to the start of the zone announcement.
        /// </summary>
        [Description("The message to append to the start of the zone announcement.")]
        public string StartupNoise { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the announcements for each zone.
        /// </summary>
        [Description("The announcements for each zone. Use $PLAYERS to indicate the number of players.")]
        public Dictionary<ZoneType, string> Announcements { get; set; } = new()
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
        [Description("Whether the cassie noise will be suppressed.")]
        public bool SuppressCassieNoise { get; set; } = true;

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.ForceAnnouncerCommand"/> class.
        /// </summary>
        public ForceAnnouncerCommand ForceAnnouncerCommand { get; set; } = new();
    }
}