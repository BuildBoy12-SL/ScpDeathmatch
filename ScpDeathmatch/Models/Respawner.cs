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
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.CustomItems;
    using ScpDeathmatch.Enums;
    using UnityEngine;

    /// <summary>
    /// Handles the respawning of players.
    /// </summary>
    public class Respawner : IDisposable
    {
        private readonly RoleType roleType;
        private readonly List<Item> items;
        private readonly Vector3 position;
        private readonly ZoneType zone;
        private readonly IEnumerable<PlayerEffect> effects;
        private bool isDisposed;
        private bool isRespawning;

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

            Player.ClearInventory();

            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
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
            if (isDisposed)
                return;

            isRespawning = true;
            Player.Role.Type = roleType;
        }

        /// <summary>
        /// Spawns the player's items where they originally died.
        /// </summary>
        public void Fail()
        {
            if (isDisposed)
                return;

            foreach (Item item in items)
            {
                if (CustomItem.TryGet(item, out CustomItem customItem) && customItem is SecondWind)
                    continue;

                item.Spawn(position);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (isDisposed)
                return;

            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            isDisposed = true;
        }

        private void OnSpawned(SpawnedEventArgs ev)
        {
            if (isDisposed || !isRespawning || ev.Player != Player)
                return;

            Player.ClearInventory();
            foreach (Item item in items)
            {
                if (CustomItem.TryGet(item, out CustomItem customItem))
                {
                    if (customItem is not SecondWind)
                        customItem.Give(Player);

                    continue;
                }

                Player.AddItem(item.Type);
            }

            foreach (PlayerEffect playerEffect in effects)
                Player.EnableEffect(playerEffect);

            Vector3 newPosition = TeleportType switch
            {
                TeleportType.Role => TeleportPosition.Get(roleType),
                TeleportType.Zone => TeleportPosition.Get(zone),
                _ => position
            };

            Player.Position = newPosition + Vector3.up;
            isRespawning = false;
            Dispose();
        }
    }
}