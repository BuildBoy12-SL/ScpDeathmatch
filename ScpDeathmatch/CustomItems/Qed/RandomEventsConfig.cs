// -----------------------------------------------------------------------
// <copyright file="RandomEventsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed
{
    using System.Collections.Generic;
    using System.Linq;
    using ScpDeathmatch.CustomItems.Qed.Models;
    using ScpDeathmatch.CustomItems.Qed.RandomEvents;
    using UnityEngine;

    /// <summary>
    /// Handles configs related to random events.
    /// </summary>
    public class RandomEventsConfig
    {
        private readonly List<IRandomEvent> selectableEvents = new List<IRandomEvent>();

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.LockRoom"/> event.
        /// </summary>
        public List<LockRoom> LockRoom { get; set; } = new List<LockRoom>
        {
            new LockRoom(),
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.PowerOutage"/> event.
        /// </summary>
        public List<PowerOutage> PowerOutage { get; set; } = new List<PowerOutage>
        {
            new PowerOutage(),
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.SpawnGrenade"/> event.
        /// </summary>
        public List<SpawnGrenade> SpawnGrenade { get; set; } = new List<SpawnGrenade>
        {
            new SpawnGrenade(),
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.SpawnItems"/> event.
        /// </summary>
        public List<SpawnItems> SpawnItems { get; set; } = new List<SpawnItems>
        {
            new SpawnItems
            {
                Name = "Spawn Scp Items",
                PossibleItems = new List<ItemPair>
                {
                    new ItemPair(ItemType.SCP207, 1),
                    new ItemPair(ItemType.SCP268, 1),
                    new ItemPair(ItemType.SCP500, 2),
                },
            },
            new SpawnItems
            {
                Name = "Spawn Weapons",
                PossibleItems = new List<ItemPair>
                {
                    new ItemPair(ItemType.GunRevolver, 1),
                    new ItemPair(ItemType.GunRevolver, 2),
                    new ItemPair(ItemType.GunE11SR, 1),
                },
            },
            new SpawnItems
            {
                Name = "Spawn Medical Items",
                PossibleItems = new List<ItemPair>
                {
                    new ItemPair(ItemType.Adrenaline, 2),
                    new ItemPair(ItemType.Medkit, 4),
                    new ItemPair(ItemType.Painkillers, 6),
                },
            },
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.SpawnScp018"/> event.
        /// </summary>
        public List<SpawnScp018> Scp018 { get; set; } = new List<SpawnScp018>
        {
            new SpawnScp018(),
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.UpgradeItems"/> event.
        /// </summary>
        public List<UpgradeItems> UpgradeItems { get; set; } = new List<UpgradeItems>
        {
            new UpgradeItems(),
        };

        /// <summary>
        /// Reloads the selectable events collection.
        /// </summary>
        public void Reload()
        {
            selectableEvents.Clear();
            selectableEvents.AddRange(LockRoom.Where(x => x.IsEnabled));
            selectableEvents.AddRange(PowerOutage.Where(x => x.IsEnabled));
            selectableEvents.AddRange(SpawnGrenade.Where(x => x.IsEnabled));
            selectableEvents.AddRange(SpawnItems.Where(x => x.IsEnabled));
            selectableEvents.AddRange(Scp018.Where(x => x.IsEnabled));
            selectableEvents.AddRange(UpgradeItems.Where(x => x.IsEnabled));
        }

        /// <summary>
        /// Finds a random event from the enabled events.
        /// </summary>
        /// <returns>The found event, or null if there are no enabled events.</returns>
        public IRandomEvent FindRandom() => selectableEvents.IsEmpty() ? null : selectableEvents[Random.Range(0, selectableEvents.Count)];
    }
}