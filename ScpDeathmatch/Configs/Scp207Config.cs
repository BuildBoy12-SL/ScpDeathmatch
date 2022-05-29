// -----------------------------------------------------------------------
// <copyright file="Scp207Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using CustomPlayerEffects;

    /// <summary>
    /// Handles configs related to Scp207.
    /// </summary>
    public class Scp207Config
    {
        /// <summary>
        /// Gets or sets the cola effect stack settings.
        /// </summary>
        public List<Scp207.NumberOfDrinks> ColaEffects { get; set; } = new()
        {
            new Scp207.NumberOfDrinks
            {
                SpeedMultiplier = 1.15f,
                DamageMultiplier = 1f,
                PostProcessIntensity = 0f,
            },
            new Scp207.NumberOfDrinks
            {
                SpeedMultiplier = 1.3f,
                DamageMultiplier = 1f,
                PostProcessIntensity = 0f,
            },
            new Scp207.NumberOfDrinks
            {
                SpeedMultiplier = 1.45f,
                DamageMultiplier = 1.5f,
                PostProcessIntensity = 0.04f,
            },
            new Scp207.NumberOfDrinks
            {
                SpeedMultiplier = 1.6f,
                DamageMultiplier = 2.5f,
                PostProcessIntensity = 0.06f,
            },
            new Scp207.NumberOfDrinks
            {
                SpeedMultiplier = 1.8f,
                DamageMultiplier = 4f,
                PostProcessIntensity = 0.08f,
            },
            new Scp207.NumberOfDrinks
            {
                SpeedMultiplier = 2f,
                DamageMultiplier = 5f,
                PostProcessIntensity = 0.1f,
            },
        };
    }
}