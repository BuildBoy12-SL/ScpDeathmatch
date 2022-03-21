// -----------------------------------------------------------------------
// <copyright file="Respawner.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.CustomItems.API.Features;
    using MEC;
    using ScpDeathmatch.CustomItems;
    using ScpDeathmatch.Enums;
    using UnityEngine;

    /// <summary>
    /// Handles the respawning of players.
    /// </summary>
    public class Respawner
    {
        private readonly RoleType roleType;
        private readonly List<Item> items;
        private readonly Vector3 position;
        private readonly ZoneType zone;
        private readonly IEnumerable<PlayerEffect> effects;

        /// <summary>
        /// Initializes a new instance of the <see cref="Respawner"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="verifyCondition"><inheritdoc cref="VerifyCondition"/></param>
        /// <param name="teleportType"><inheritdoc cref="TeleportType"/></param>
        public Respawner(Player player, Func<bool> verifyCondition, TeleportType teleportType)
        {
            Player = player;
            VerifyCondition = verifyCondition;
            TeleportType = teleportType;

            roleType = player.Role.Type;
            items = player.Items.ToList();
            position = player.Position;
            zone = player.Zone;
            effects = player.ActiveEffects;

            player.ClearInventory();
        }

        /// <summary>
        /// Gets the player to respawn.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the function to verify that the player should respawn.
        /// </summary>
        public Func<bool> VerifyCondition { get; }

        /// <summary>
        /// Gets the type of teleportation to use.
        /// </summary>
        public TeleportType TeleportType { get; }

        /// <summary>
        /// Respawns the player.
        /// </summary>
        public void Respawn()
        {
            Player.Role.Type = roleType;
            Timing.CallDelayed(1f, () =>
            {
                foreach (Item item in items)
                {
                    if (!(CustomItem.TryGet(item, out CustomItem customItem) && customItem is SecondWind))
                        Player.AddItem(item);
                }

                Player.Inventory.SendItemsNextFrame = true;
                foreach (PlayerEffect playerEffect in effects)
                    Player.ReferenceHub.playerEffectsController.EnableEffect(playerEffect);

                Vector3 newPosition = Vector3.zero;
                if (TeleportType == TeleportType.Role)
                    newPosition = TeleportPosition.Get(roleType);
                else if (TeleportType == TeleportType.Zone)
                    newPosition = TeleportPosition.Get(zone);

                Player.Position = (newPosition != Vector3.zero ? newPosition : position) + Vector3.up;
            });
        }

        /// <summary>
        /// Spawns the player's items where they originally died.
        /// </summary>
        public void Fail()
        {
            foreach (Item item in items)
            {
                if (CustomItem.TryGet(item, out CustomItem customItem) && customItem is SecondWind)
                    continue;

                item.Spawn(position);
            }
        }
    }
}