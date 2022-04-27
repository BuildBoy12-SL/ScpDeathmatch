// -----------------------------------------------------------------------
// <copyright file="StatsDatabaseConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.IO;
    using Exiled.API.Features;
    using ScpDeathmatch.Stats.Commands;

    /// <summary>
    /// Handles configs related to the stats database.
    /// </summary>
    public class StatsDatabaseConfig
    {
        /// <summary>
        /// Gets or sets the directory to the database.
        /// </summary>
        public string DirectoryPath { get; set; } = Path.Combine(Paths.Configs, "ScpDeathmatch", "Database");

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string FileName { get; set; } = "Stats.db";

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Stats.Commands.ClearStatsCommand"/> class.
        /// </summary>
        public ClearStatsCommand ClearStatsCommand { get; set; } = new();
    }
}