// -----------------------------------------------------------------------
// <copyright file="UpgradeSetting.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using System.Collections.Generic;
    using Scp914;

    /// <summary>
    /// Represents a chance that an item will upgrade to something on a certain setting.
    /// </summary>
    public class UpgradeSetting
    {
        /// <summary>
        /// Gets or sets the chances for each knob setting to succeed.
        /// </summary>
        public Dictionary<Scp914KnobSetting, int> Chances { get; set; } = new Dictionary<Scp914KnobSetting, int>
        {
            { Scp914KnobSetting.Rough, 0 },
            { Scp914KnobSetting.Coarse, 0 },
            { Scp914KnobSetting.OneToOne, 0 },
            { Scp914KnobSetting.Fine, 0 },
            { Scp914KnobSetting.VeryFine, 0 },
        };

        /// <summary>
        /// Compares the knobs chance against a random number to see if the upgrade chance passed.
        /// </summary>
        /// <param name="scp914KnobSetting">The setting to check.</param>
        /// <returns>A value indicating whether the check passed.</returns>
        public bool Check(Scp914KnobSetting scp914KnobSetting) => Chances.TryGetValue(scp914KnobSetting, out int chance) && Exiled.Loader.Loader.Random.Next(100) < chance;
    }
}