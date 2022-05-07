// -----------------------------------------------------------------------
// <copyright file="RandomTeleport.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using UnityEngine;

    /// <inheritdoc />
    public class RandomTeleport : IRandomEvent
    {
        /// <inheritdoc />
        public string Name { get; set; } = nameof(RandomTeleport);

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public float Weight { get; set; } = 0.5f;

        /// <summary>
        /// Gets or sets the maximum distance the player should be from the grenade to be teleported.
        /// </summary>
        public float MaxDistance { get; set; } = 3;

        /// <summary>
        /// Gets or sets a value indicating whether the player should be teleported to a room in the same zone.
        /// </summary>
        public bool SameZone { get; set; } = true;

        /// <inheritdoc />
        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            foreach (Player player in Player.List)
            {
                if (player.SessionVariables.ContainsKey("IsNPC"))
                    continue;

                if ((player.Position - ev.Grenade.transform.position).magnitude > MaxDistance * MaxDistance)
                    continue;

                Room room;
                Vector3 newPosition;

                do
                {
                    room = SameZone ? Room.Random(player.Zone) : Room.Random();
                }
                while (!PlayerMovementSync.FindSafePosition(room.Position + Vector3.up, out newPosition));

                player.Teleport(newPosition);
            }
        }
    }
}