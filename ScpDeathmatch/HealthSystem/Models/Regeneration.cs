// -----------------------------------------------------------------------
// <copyright file="Regeneration.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem.Models
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using InventorySystem.Items.Usables;
    using UnityEngine;

    /// <summary>
    /// Represents an applicable regeneration process.
    /// </summary>
    public class Regeneration
    {
        private AnimationCurve regenerationCurve = new();
        private SortedList<float, float> rawRegenerationCurve = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Regeneration"/> class.
        /// </summary>
        public Regeneration()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Regeneration"/> class.
        /// </summary>
        /// <param name="regenerationCurve"><inheritdoc cref="RegenerationCurve"/></param>
        public Regeneration(SortedList<float, float> regenerationCurve)
        {
            RegenerationCurve = regenerationCurve;
        }

        /// <summary>
        /// Gets or sets the curve of the regeneration.
        /// </summary>
        public SortedList<float, float> RegenerationCurve
        {
            get => rawRegenerationCurve;
            set
            {
                rawRegenerationCurve = value;
                regenerationCurve = new AnimationCurve();
                foreach (KeyValuePair<float, float> kvp in value)
                    regenerationCurve.AddKey(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Gets or sets the speed multiplier.
        /// </summary>
        public float SpeedMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the health multiplier.
        /// </summary>
        public float HealthMultiplier { get; set; }

        /// <summary>
        /// Applies the regeneration process to the specified player.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to apply the regeneration to.</param>
        public void ApplyTo(Player player)
        {
            if (player is not null)
                UsableItemsController.GetHandler(player.ReferenceHub).ActiveRegenerations.Add(new RegenerationProcess(regenerationCurve, SpeedMultiplier, HealthMultiplier));
        }
    }
}