// -----------------------------------------------------------------------
// <copyright file="SpawnWeapon.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using System.Collections.Generic;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using UnityEngine;

    /// <inheritdoc />
    public class SpawnWeapon : IRandomEvent
    {
        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public string Name { get; set; } = nameof(SpawnWeapon);

        /// <summary>
        /// Gets or sets the items that can spawn.
        /// </summary>
        public List<ItemType> PossibleItems { get; set; } = new List<ItemType>
        {
            ItemType.GunRevolver,
            ItemType.GunRevolver,
            ItemType.GunE11SR,
        };

        /// <inheritdoc />
        public void Action(ExplodingGrenadeEventArgs ev)
        {
            ev.TargetsToAffect.Clear();
            if (PossibleItems == null || PossibleItems.IsEmpty())
                return;

            Item.Create(PossibleItems[Random.Range(0, PossibleItems.Count)]).Spawn(ev.Grenade.transform.position);
        }
    }
}