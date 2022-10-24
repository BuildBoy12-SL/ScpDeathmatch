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
    using ScpDeathmatch.ItemThrowing.Models;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles configs related to item throwing.
    /// </summary>
    public class ItemThrowingConfig
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
                    EnemySettings = new EnemySettings
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
                    FriendlySettings = new FriendlySettings()
                    {
                        Heal = new FloatRange(10f, 15f),
                    },
                }
            },
            {
                ItemType.Adrenaline, new ThrowSettings
                {
                    FriendlySettings = new FriendlySettings()
                    {
                        Heal = new FloatRange(10f, 15f),
                        AhpHeal = new FloatRange(20f, 20f),
                    },
                }
            },
        };
    }
}