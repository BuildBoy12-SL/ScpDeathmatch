// -----------------------------------------------------------------------
// <copyright file="MiscEffectsMicroConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs.MicroHidConfigs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles configs that apply misc. effects to players using the Micro HID.
    /// </summary>
    public class MiscEffectsMicroConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the speedy micro is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the effects to apply.
        /// </summary>
        [Description("The effects to apply. Duration is ignored.")]
        public List<ConfiguredEffect> Effects { get; set; } = new()
        {
            new ConfiguredEffect(EffectType.Invigorated, 1, 0f),
        };
    }
}