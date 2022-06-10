// -----------------------------------------------------------------------
// <copyright file="Range.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.Models
{
    using System;
    using ScpDeathmatch.CustomItems.Qed.Enums;

    /// <summary>
    /// Represents ranges of values.
    /// </summary>
    [Serializable]
    public class Range
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Range"/> class.
        /// </summary>
        public Range()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Range"/> class.
        /// </summary>
        /// <param name="neutral"><inheritdoc cref="Neutral"/></param>
        /// <param name="far"><inheritdoc cref="Far"/></param>
        /// <param name="close"><inheritdoc cref="Close"/></param>
        public Range(float neutral, float far, float close = 0f)
        {
            Neutral = neutral;
            Far = far;
            Close = close;
        }

        /// <summary>
        /// Gets or sets the minimum value for close.
        /// </summary>
        public float Close { get; set; }

        /// <summary>
        /// Gets or sets the minimum value for neutral.
        /// </summary>
        public float Neutral { get; set; }

        /// <summary>
        /// Gets or sets the minimum value for far.
        /// </summary>
        public float Far { get; set; }

        /// <summary>
        /// Evaluates the distance and returns the appropriate <see cref="RangeType"/>.
        /// </summary>
        /// <param name="distance">The distance to evaluate.</param>
        /// <returns>The range that mirrors the configured value.</returns>
        public RangeType Evaluate(float distance)
        {
            if (distance >= Close && distance <= Neutral)
                return RangeType.Close;

            if (distance >= Neutral && distance <= Far)
                return RangeType.Neutral;

            return RangeType.Far;
        }
    }
}