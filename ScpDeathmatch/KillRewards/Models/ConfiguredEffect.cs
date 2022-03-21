// -----------------------------------------------------------------------
// <copyright file="ConfiguredEffect.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.KillRewards.Models
{
    using System;
    using Exiled.API.Enums;

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
        /// <param name="type"><inheritdoc cref="Type"/></param>
        /// <param name="duration"><inheritdoc cref="Duration"/></param>
        /// <param name="intensity"><inheritdoc cref="Intensity"/></param>
        public ConfiguredEffect(EffectType type, float duration, byte intensity)
        {
            Type = type;
            Duration = duration;
            Intensity = intensity;
        }

        /// <summary>
        /// Gets or sets the effect.
        /// </summary>
        public EffectType Type { get; set; }

        /// <summary>
        /// Gets or sets the effects duration.
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Gets or sets the effects intensity.
        /// </summary>
        public byte Intensity { get; set; }

        /// <summary>
        /// Adds two effects together.
        /// </summary>
        /// <param name="a">The first effect.</param>
        /// <param name="b">The second effect.</param>
        /// <returns>An effect with the added duration and intensities, or null if they are not of the same type.</returns>
        public static ConfiguredEffect operator +(ConfiguredEffect a, ConfiguredEffect b)
        {
            if (a.Type != b.Type)
                return null;

            return new ConfiguredEffect(a.Type, a.Duration + b.Duration, (byte)(a.Intensity + b.Intensity));
        }
    }
}