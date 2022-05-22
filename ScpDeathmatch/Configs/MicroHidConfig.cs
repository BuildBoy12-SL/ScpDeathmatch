// -----------------------------------------------------------------------
// <copyright file="MicroHidConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using ScpDeathmatch.Configs.MicroHidConfigs;

    /// <summary>
    /// Handles all micro-hid configurations.
    /// </summary>
    public class MicroHidConfig
    {
        /// <summary>
        /// Gets or sets the configs for healing while using the micro.
        /// </summary>
        public HealingMicroConfig Healing { get; set; } = new();

        /// <summary>
        /// Gets or sets the configs for misc. effects while using the micro.
        /// </summary>
        public MiscEffectsMicroConfig MiscEffects { get; set; } = new();

        /// <summary>
        /// Gets or sets the configs for additional movement speed while using the micro.
        /// </summary>
        public SpeedyMicroConfig Speed { get; set; } = new();
    }
}