// -----------------------------------------------------------------------
// <copyright file="FloatRange.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    /// <summary>
    /// Represents a range between two floats.
    /// </summary>
    public class FloatRange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FloatRange"/> class.
        /// </summary>
        public FloatRange()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatRange"/> class.
        /// </summary>
        /// <param name="minimum"><inheritdoc cref="Minimum"/></param>
        /// <param name="maximum"><inheritdoc cref="Maximum"/></param>
        public FloatRange(float minimum, float maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public float Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public float Maximum { get; set; }

        /// <summary>
        /// Gets a random value between the minimum and maximum values.
        /// </summary>
        /// <returns>The random value as a float.</returns>
        public float GetRandomValue() => UnityEngine.Random.Range(Minimum, Maximum);
    }
}