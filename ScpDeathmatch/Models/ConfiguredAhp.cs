// -----------------------------------------------------------------------
// <copyright file="ConfiguredAhp.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    /// <summary>
    /// Represents a configured ahp model.
    /// </summary>
    public class ConfiguredAhp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredAhp"/> class.
        /// </summary>
        public ConfiguredAhp()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredAhp"/> class.
        /// </summary>
        /// <param name="limit"><inheritdoc cref="Limit"/></param>
        /// <param name="decayRate"><inheritdoc cref="DecayRate"/></param>
        /// <param name="efficacy"><inheritdoc cref="Efficacy"/></param>
        public ConfiguredAhp(float limit, float decayRate, float efficacy)
        {
            Limit = limit;
            DecayRate = decayRate;
            Efficacy = efficacy;
        }

        /// <summary>
        /// Gets or sets the maximum ahp.
        /// </summary>
        public float Limit { get; set; }

        /// <summary>
        /// Gets or sets the decay rate.
        /// </summary>
        public float DecayRate { get; set; }

        /// <summary>
        /// Gets or sets the efficacy of the ahp.
        /// </summary>
        public float Efficacy { get; set; }
    }
}