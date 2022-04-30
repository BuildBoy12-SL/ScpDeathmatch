// -----------------------------------------------------------------------
// <copyright file="SpawnItems.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using System.Collections.Generic;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
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
        public float Weight { get; set; }

        /// <summary>
        /// Gets or sets the items that can spawn with the amount to spawn.
        /// </summary>
        public List<ItemPair> PossibleItems { get; set; } = new();

        /// <inheritdoc />
        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            if (PossibleItems is null || PossibleItems.IsEmpty())
                return;

            ItemPair pair = PossibleItems[Random.Range(0, PossibleItems.Count)];
            Vector3 spawnPosition = ev.Grenade.transform.position + (Vector3.up * 2);
            for (int i = 0; i < pair.Amount; i++)
                Item.Create(pair.Item).Spawn(spawnPosition);
        }
    }
}