// -----------------------------------------------------------------------
// <copyright file="TeleportPosition.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using ScpDeathmatch.Enums;
    using UnityEngine;

    /// <summary>
    /// A helper for getting available teleportation positions.
    /// </summary>
    public static class TeleportPosition
    {
        /// <summary>
        /// Gets an available teleportation position.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to teleport.</param>
        /// <param name="teleportType">The type of teleportation.</param>
        /// <returns>A <see cref="Vector3"/> that represents the selected position.</returns>
        public static Vector3 Get(Player player, TeleportType teleportType)
        {
            switch (teleportType)
            {
                case TeleportType.Zone:
                    return Get(player.Zone);
                case TeleportType.Role:
                    return Get(player.Role.Type);
                case TeleportType.None:
                default:
                    return Vector3.zero;
            }
        }

        /// <summary>
        /// Gets an available teleportation position.
        /// </summary>
        /// <param name="zoneType">The zone to teleport within.</param>
        /// <returns>A <see cref="Vector3"/> that represents the selected position.</returns>
        public static Vector3 Get(ZoneType zoneType)
            => Room.Random(zoneType).Position;

        /// <summary>
        /// Gets an available teleportation position.
        /// </summary>
        /// <param name="roleType">The role spawn to teleport to.</param>
        /// <returns>A <see cref="Vector3"/> that represents the selected position.</returns>
        public static Vector3 Get(RoleType roleType)
            => roleType.GetRandomSpawnProperties().Item1;
    }
}