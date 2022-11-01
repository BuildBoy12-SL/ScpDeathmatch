// -----------------------------------------------------------------------
// <copyright file="MapGenerationConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using MapGeneration;
    using MapGeneration.Distributors;
    using ScpDeathmatch.API.Interfaces;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles configs related to map generation.
    /// </summary>
    public class MapGenerationConfig : IConfigFile
    {
        /// <summary>
        /// Gets or sets the blacklist for spawning structures in specific rooms.
        /// </summary>
        [Description("The blacklist for spawning structures in specific rooms.")]
        public Dictionary<StructureType, List<RoomName>> SpawnBlacklist { get; set; } = new()
        {
            {
                StructureType.Scp079Generator, new List<RoomName>
                {
                    RoomName.HczWarhead,
                }
            },
        };

        /// <summary>
        /// Gets or sets adjusted minimum and maximum structure spawn counts.
        /// </summary>
        [Description("Adjusted minimum and maximum structure spawn counts.")]
        public Dictionary<StructureType, Limit> StructureLimits { get; set; } = new()
        {
            { StructureType.Scp079Generator, new Limit(3, 3) },
        };
    }
}