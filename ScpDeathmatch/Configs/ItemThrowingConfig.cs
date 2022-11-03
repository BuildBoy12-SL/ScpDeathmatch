// -----------------------------------------------------------------------
// <copyright file="ItemThrowingConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using ScpDeathmatch.API.Attributes;
    using ScpDeathmatch.API.Interfaces;
    using ScpDeathmatch.ItemThrowing.Models;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles configs related to item throwing.
    /// </summary>
    [NestedConfig]
    public class ItemThrowingConfig : IConfigFile
    {
        /// <summary>
        /// Gets or sets a value indicating whether debug logs should be enabled.
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Gets or sets a collection pairing item types to their respective throwing configs.
        /// </summary>
        public Dictionary<ItemType, ThrowSettings> Settings { get; set; } = new()
        {
            {
                ItemType.MicroHID, new ThrowSettings
                {
                    EnemySettings = new ItemSettings
                    {
                        Damage = new FloatRange(150f, 200f),
                        Effects = new[]
                        {
                            new ConfiguredEffect(EffectType.Concussed, 1, 10f),
                        },
                        HitMultiple = true,
                    },
                }
            },
            {
                ItemType.Painkillers, new ThrowSettings
                {
                    FriendlySettings = new ItemSettings
                    {
                        Heal = new FloatRange(10f, 15f),
                        HealHint = new Hint("{0} healed you with painkillers", 3f),
                    },
                }
            },
            {
                ItemType.Adrenaline, new ThrowSettings
                {
                    FriendlySettings = new ItemSettings
                    {
                        Heal = new FloatRange(10f, 15f),
                        AhpHeal = new FloatRange(20f, 20f),
                        HealHint = new Hint("{0} healed you with adrenaline", 3f),
                    },
                }
            },
        };
    }
}