// -----------------------------------------------------------------------
// <copyright file="SpawnItems.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using System;
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.CustomItems.Qed.Enums;
    using ScpDeathmatch.CustomItems.Qed.Models;
    using UnityEngine;

    /// <inheritdoc />
    public class SpawnItems : IRandomEvent
    {
        /// <inheritdoc />
        public string Name { get; set; } = nameof(SpawnItems);

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public RangeType Range { get; set; } = RangeType.Neutral;

        /// <summary>
        /// Gets or sets the items that can spawn with the amount to spawn.
        /// </summary>
        public List<ItemPair> PossibleItems { get; set; } = new();

        /// <inheritdoc />
        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            if (PossibleItems is null || PossibleItems.IsEmpty())
                return;

            ItemPair pair = PossibleItems[UnityEngine.Random.Range(0, PossibleItems.Count)];
            Vector3 spawnPosition = ev.Grenade.transform.position + (Vector3.up * 2);
            for (int i = 0; i < pair.Amount; i++)
                SpawnItem(pair.Item, spawnPosition);
        }

        private void SpawnItem(string itemName, Vector3 position)
        {
            if (CustomItem.TryGet(itemName, out CustomItem customItem))
            {
                customItem.Spawn(position, (Player)null);
                return;
            }

            if (Enum.TryParse(itemName, out ItemType type))
            {
                Item.Create(type).Spawn(position);
                return;
            }

            Log.Warn($"{Name}: {nameof(SpawnItem)}: {itemName} is not a valid ItemType or Custom Item name.");
        }
    }
}