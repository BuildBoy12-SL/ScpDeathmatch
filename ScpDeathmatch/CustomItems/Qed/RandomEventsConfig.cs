// -----------------------------------------------------------------------
// <copyright file="RandomEventsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.CustomItems.Qed.Models;
    using ScpDeathmatch.CustomItems.Qed.RandomEvents;
    using UnityEngine;

    /// <summary>
    /// Handles configs related to random events.
    /// </summary>
    public class RandomEventsConfig
    {
        private readonly List<IRandomEvent> selectableEvents = new();
        private AnimationCurve eventChanceCurve = new();

        /// <summary>
        /// Gets or sets the list of distance to weight pairs that determines the chances of random events being triggered.
        /// </summary>
        [Description("The list of distance to weight pairs that determines the chances of random events being triggered.")]
        public Dictionary<float, float> WeightCurve { get; set; } = new()
        {
            { 0f, 1f },
            { 15f, 0f },
            { 30f, -1f },
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.LockRoom"/> event.
        /// </summary>
        public List<LockRoom> LockRoom { get; set; } = new()
        {
            new LockRoom(),
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.PowerOutage"/> event.
        /// </summary>
        public List<PowerOutage> PowerOutage { get; set; } = new()
        {
            new PowerOutage(),
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.RandomTeleport"/> event.
        /// </summary>
        public List<RandomTeleport> RandomTeleport { get; set; } = new()
        {
            new RandomTeleport(),
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.SpawnGrenade"/> event.
        /// </summary>
        public List<SpawnGrenade> SpawnGrenade { get; set; } = new()
        {
            new SpawnGrenade(),
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.SpawnItems"/> event.
        /// </summary>
        public List<SpawnItems> SpawnItems { get; set; } = new()
        {
            new SpawnItems
            {
                Name = "Spawn Scp Items",
                PossibleItems = new List<ItemPair>
                {
                    new(ItemType.SCP207, 1),
                    new(ItemType.SCP268, 1),
                    new(ItemType.SCP500, 2),
                },
            },
            new SpawnItems
            {
                Name = "Spawn Weapons",
                PossibleItems = new List<ItemPair>
                {
                    new(ItemType.GunRevolver, 1),
                    new(ItemType.GunRevolver, 2),
                    new(ItemType.GunE11SR, 1),
                },
            },
            new SpawnItems
            {
                Name = "Spawn Medical Items",
                PossibleItems = new List<ItemPair>
                {
                    new(ItemType.Adrenaline, 2),
                    new(ItemType.Medkit, 4),
                    new(ItemType.Painkillers, 6),
                },
            },
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.SpawnScp018"/> event.
        /// </summary>
        public List<SpawnScp018> Scp018 { get; set; } = new()
        {
            new SpawnScp018(),
        };

        /// <summary>
        /// Gets or sets a collection of the <see cref="RandomEvents.UpgradeItems"/> event.
        /// </summary>
        public List<UpgradeItems> UpgradeItems { get; set; } = new()
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
            selectableEvents.AddRange(RandomTeleport.Where(x => x.IsEnabled));
            selectableEvents.AddRange(SpawnGrenade.Where(x => x.IsEnabled));
            selectableEvents.AddRange(SpawnItems.Where(x => x.IsEnabled));
            selectableEvents.AddRange(Scp018.Where(x => x.IsEnabled));
            selectableEvents.AddRange(UpgradeItems.Where(x => x.IsEnabled));

            eventChanceCurve = new AnimationCurve();
            foreach (KeyValuePair<float, float> kvp in WeightCurve)
                eventChanceCurve.AddKey(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Finds a random event from the enabled events.
        /// </summary>
        /// <param name="ev">The explosion that initiated the random event.</param>
        /// <returns>The found event, or null if there are no enabled events.</returns>
        public IRandomEvent FindRandom(ExplodingGrenadeEventArgs ev)
        {
            if (selectableEvents.IsEmpty())
                return null;

            float distance = Vector3.Distance(ev.Thrower.Position, ev.Grenade.transform.position);
            float evaluatedWeight = eventChanceCurve.Evaluate(distance);

            float minimumWeight = selectableEvents.Min(x => x.Weight);
            float maximumWeight = selectableEvents.Max(x => x.Weight);
            float bounds = Math.Abs(maximumWeight - minimumWeight) / (selectableEvents.Count / 2f);

            List<IRandomEvent> randomEvents = selectableEvents.Where(randomEvent => Math.Abs(randomEvent.Weight - evaluatedWeight) < bounds).ToList();
            return randomEvents.IsEmpty() ? null : randomEvents[UnityEngine.Random.Range(0, randomEvents.Count)];
        }
    }
}