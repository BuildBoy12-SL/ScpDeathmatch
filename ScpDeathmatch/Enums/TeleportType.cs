// -----------------------------------------------------------------------
// <copyright file="TeleportType.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Enums
{
    /// <summary>
    /// Defines various types of teleportation when a player gets a new life.
    /// </summary>
    public enum TeleportType
    {
        /// <summary>
        /// Represents no teleportation taking place.
        /// </summary>
        None,

        /// <summary>
        /// Represents teleportation within the same zone.
        /// </summary>
        Zone,

        /// <summary>
        /// Represents teleportation to a spawnpoint owned by the role.
        /// </summary>
        Role,
    }
}