// -----------------------------------------------------------------------
// <copyright file="Scp207Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using ScpDeathmatch.API.Interfaces;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles configs related to Scp207.
    /// </summary>
    public class Scp207Config : IConfigFile
    {
        /// <summary>
        /// Gets or sets the cola effect stack settings.
        /// </summary>
        public List<ColaEffect> ColaEffects { get; set; } = new()
        {
            new ColaEffect(1f, 1f),
            new ColaEffect(1.15f, 1.25f),
            new ColaEffect(1.3f, 1.5f),
            new ColaEffect(1.45f, 2f),
            new ColaEffect(1.6f, 3f),
            new ColaEffect(1.8f, 4f),
            new ColaEffect(2f, 5f),
        };
    }
}