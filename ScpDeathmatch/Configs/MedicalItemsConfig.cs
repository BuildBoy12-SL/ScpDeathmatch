// -----------------------------------------------------------------------
// <copyright file="MedicalItemsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using InventorySystem.Items.Usables;
    using ScpDeathmatch.HealthSystem.Models;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles the configs for modifying consumable medical items.
    /// </summary>
    public class MedicalItemsConfig
    {
        private Dictionary<Type, PropertyInfo> cachedProperties;

        /// <summary>
        /// Gets or sets the actions to run when <see cref="InventorySystem.Items.Usables.Adrenaline"/> is used.
        /// </summary>
        public MedicalActions Adrenaline { get; set; } = new()
        {
            AddedEffects = new List<ConfiguredEffect>
            {
                new(EffectType.MovementBoost, 20, 8f, true),
                new(EffectType.Invigorated, 1, 8f, true),
            },
            RegeneratedStamina = 100f,
        };

        /// <summary>
        /// Gets or sets the actions to run when a <see cref="InventorySystem.Items.Usables.Medkit"/> is used.
        /// </summary>
        public MedicalActions Medkit { get; set; } = new()
        {
            InstantHealth = 100,
            InstantMaxHealth = 100,
        };

        /// <summary>
        /// Gets or sets the actions to run when a <see cref="InventorySystem.Items.Usables.Painkillers"/> is used.
        /// </summary>
        public MedicalActions Painkillers { get; set; } = new()
        {
            Ahp = new ConfiguredAhp(25f, sustain: 10f),
        };

        /// <summary>
        /// Gets or sets the actions to run when a <see cref="InventorySystem.Items.Usables.Scp500"/> is used.
        /// </summary>
        public MedicalActions Scp500 { get; set; } = new()
        {
            AddedEffects = new List<ConfiguredEffect>()
            {
                new(EffectType.MovementBoost, 50, 10f),
            },
            Ahp = new ConfiguredAhp(50f, sustain: 10f),
            DisarmDuration = 10f,
        };

        /// <summary>
        /// Gets the corresponding medical action with the specified consumable.
        /// </summary>
        /// <param name="consumable">The type of consumable.</param>
        /// <returns>The corresponding medical actions, or null if one is not found.</returns>
        public MedicalActions GetActions(Consumable consumable)
        {
            cachedProperties ??= GenerateCache();
            Log.Warn(consumable.GetType());
            if (cachedProperties.TryGetValue(consumable.GetType(), out PropertyInfo property))
            {
                Log.Warn(property.Name);
                return property.GetValue(this) as MedicalActions;
            }

            return null;
        }

        private Dictionary<Type, PropertyInfo> GenerateCache()
        {
            Dictionary<Type, PropertyInfo> cache = new();

            List<Type> consumables = typeof(Consumable).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Consumable))).ToList();
            foreach (PropertyInfo property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.PropertyType != typeof(MedicalActions))
                    continue;

                foreach (Type type in consumables)
                {
                    if (type.Name.Equals(property.Name))
                    {
                        cache.Add(type, property);
                        break;
                    }
                }
            }

            return cache;
        }
    }
}