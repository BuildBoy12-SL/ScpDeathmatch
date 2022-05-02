﻿// -----------------------------------------------------------------------
// <copyright file="ConfiguredEffect.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using System;
    using Exiled.API.Enums;
    using Exiled.API.Features;

    /// <summary>
    /// Represents an effect with a configured duration and intensity.
    /// </summary>
    [Serializable]
    public class ConfiguredEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredEffect"/> class.
        /// </summary>
        public ConfiguredEffect()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredEffect"/> class.
        /// </summary>
        /// <param name="effectType"><inheritdoc cref="EffectType"/></param>
        /// <param name="intensity"><inheritdoc cref="Intensity"/></param>
        /// <param name="duration"><inheritdoc cref="Duration"/></param>
        public ConfiguredEffect(EffectType effectType, byte intensity, float duration)
        {
            Type = effectType;
            Intensity = intensity;
            Duration = duration;
        }

        /// <summary>
        /// Gets or sets the type of effect.
        /// </summary>
        public EffectType Type { get; set; }

        /// <summary>
        /// Gets or sets the intensity of the effect.
        /// </summary>
        public byte Intensity { get; set; }

        /// <summary>
        /// Gets or sets the duration of the effect.
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Applies the configured effect to the player.
        /// </summary>
        /// <param name="player">The player to apply the effect to.</param>
        public void Apply(Player player)
        {
            player.EnableEffect(Type, Duration);
            player.ChangeEffectIntensity(Type, Intensity);
        }
    }
}