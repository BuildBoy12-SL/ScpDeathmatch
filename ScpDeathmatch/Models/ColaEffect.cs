// -----------------------------------------------------------------------
// <copyright file="ColaEffect.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using CustomPlayerEffects;

    /// <summary>
    /// Represents a colas effect.
    /// </summary>
    public class ColaEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColaEffect"/> class.
        /// </summary>
        public ColaEffect()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColaEffect"/> class.
        /// </summary>
        /// <param name="speedMultiplier"><inheritdoc cref="SpeedMultiplier"/></param>
        /// <param name="damageMultiplier"><inheritdoc cref="DamageMultiplier"/></param>
        public ColaEffect(float speedMultiplier, float damageMultiplier)
        {
            SpeedMultiplier = speedMultiplier;
            DamageMultiplier = damageMultiplier;
        }

        /// <summary>
        /// Gets or sets the speed multiplier.
        /// </summary>
        public float SpeedMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the damage multiplier.
        /// </summary>
        public float DamageMultiplier { get; set; }

        /// <summary>
        /// Converts the cola effect into the <see cref="Scp207.NumberOfDrinks"/> representation.
        /// </summary>
        /// <returns>The <see cref="Scp207.NumberOfDrinks"/> conversion.</returns>
        public Scp207.NumberOfDrinks EffectConversion()
        {
            return new Scp207.NumberOfDrinks
            {
                SpeedMultiplier = SpeedMultiplier,
                DamageMultiplier = DamageMultiplier,
            };
        }
    }
}