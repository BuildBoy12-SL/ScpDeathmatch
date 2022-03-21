// -----------------------------------------------------------------------
// <copyright file="UpgradeChance.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using Scp914;
    using UnityEngine;

    /// <summary>
    /// Represents a chance for an upgrade to succeed on a specified setting.
    /// </summary>
    public class UpgradeChance
    {
        private int chance;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeChance"/> class.
        /// </summary>
        public UpgradeChance()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeChance"/> class.
        /// </summary>
        /// <param name="knobSetting"><inheritdoc cref="KnobSetting"/></param>
        /// <param name="chance"><inheritdoc cref="Chance"/></param>
        public UpgradeChance(Scp914KnobSetting knobSetting, int chance)
        {
            KnobSetting = knobSetting;
            Chance = chance;
        }

        /// <summary>
        /// Gets or sets the setting Scp914 is set to.
        /// </summary>
        public Scp914KnobSetting KnobSetting { get; set; }

        /// <summary>
        /// Gets or sets the percent chance that the upgrade will succeed.
        /// </summary>
        public int Chance
        {
            get => chance;
            set => chance = Mathf.Clamp(value, 0, 100);
        }

        /// <summary>
        /// Checks the <see cref="Chance"/> against a random number.
        /// </summary>
        /// <returns>Whether the chance was greater than the random number.</returns>
        public bool Pass() => Exiled.Loader.Loader.Random.Next(100) < Chance;
    }
}