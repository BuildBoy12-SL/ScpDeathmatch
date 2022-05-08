// -----------------------------------------------------------------------
// <copyright file="RandomTeleport.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using ScpDeathmatch.Models;
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
        [Description("The maximum distance the player should be from the grenade to be teleported.")]
        public float MaxDistance { get; set; } = 3;

        /// <summary>
        /// Gets or sets a value indicating whether the player should be teleported to a room in the same zone.
        /// </summary>
        [Description("Whether the player should be teleported to a room in the same zone.")]
        public bool SameZone { get; set; } = true;

        /// <summary>
        /// Gets or sets the delay before teleporting the affected players.
        /// </summary>
        [Description("The delay before teleporting the affected players.")]
        public float Delay { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the effects to apply to the affected players.
        /// </summary>
        [Description("The effects to apply to the affected players.")]
        public List<ConfiguredEffect> Effects { get; set; } = new();

        /// <summary>
        /// Gets or sets the rooms that players cannot be teleported to.
        /// </summary>
        [Description("The rooms that players cannot be teleported to.")]
        public List<RoomType> RoomBlacklist { get; set; } = new()
        {
            RoomType.Lcz173,
            RoomType.Hcz106,
        };

        /// <inheritdoc />
        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            foreach (Player player in Player.List)
            {
                if (player.SessionVariables.ContainsKey("IsNPC") ||
                    (player.Position - ev.Grenade.transform.position).magnitude > MaxDistance * MaxDistance)
                    continue;

                if (Effects != null)
                {
                    foreach (ConfiguredEffect effect in Effects)
                        effect.Apply(player);
                }

                Room room;
                Vector3 newPosition;

                do
                {
                    room = SameZone ? Room.Random(player.Zone) : Room.Random();
                }
                while (RoomBlacklist.Contains(room.Type) || !PlayerMovementSync.FindSafePosition(room.Position + Vector3.up, out newPosition));

                if (Delay > 0f)
                    Timing.CallDelayed(Delay, () => player.Teleport(newPosition));
                else
                    player.Teleport(newPosition);
            }
        }
    }
}