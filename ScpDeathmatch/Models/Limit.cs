// -----------------------------------------------------------------------
// <copyright file="Limit.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    /// <summary>
    /// Represents an upper and lower limit.
    /// </summary>
    public class Limit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Limit"/> class.
        /// </summary>
        public Limit()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Limit"/> class.
        /// </summary>
        /// <param name="min"><inheritdoc cref="Min"/></param>
        /// <param name="max"><inheritdoc cref="Max"/></param>
        public Limit(int min, int max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Gets or sets the lower bound.
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// Gets or sets the upper bound.
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// Checks if the given value is within the bounds.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>Whether the supplied value was within the bounds.</returns>
        public bool WithinLimit(int value) => value >= Min && value <= Max;
    }
}