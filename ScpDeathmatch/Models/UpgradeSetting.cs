// -----------------------------------------------------------------------
// <copyright file="UpgradeSetting.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Scp914;

    /// <summary>
    /// Represents a chance that an item will upgrade to something on a certain setting.
    /// </summary>
    public class UpgradeSetting
    {
        private Dictionary<Scp914KnobSetting, int> chances = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeSetting"/> class.
        /// </summary>
        public UpgradeSetting()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeSetting"/> class.
        /// </summary>
        /// <param name="chances"><inheritdoc cref="Chances"/></param>
        public UpgradeSetting(Dictionary<Scp914KnobSetting, int> chances) => Chances = chances;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeSetting"/> class.
        /// </summary>
        /// <param name="rough">The chance that the <see cref="Scp914KnobSetting.Rough"/> setting will succeed.</param>
        /// <param name="coarse">The chance that the <see cref="Scp914KnobSetting.Coarse"/> setting will succeed.</param>
        /// <param name="oneToOne">The chance that the <see cref="Scp914KnobSetting.OneToOne"/> setting will succeed.</param>
        /// <param name="fine">The chance that the <see cref="Scp914KnobSetting.Fine"/> setting will succeed.</param>
        /// <param name="veryFine">The chance that the <see cref="Scp914KnobSetting.VeryFine"/> setting will succeed.</param>
        public UpgradeSetting(int rough = 0, int coarse = 0, int oneToOne = 0, int fine = 0, int veryFine = 0)
        {
            Chances = new Dictionary<Scp914KnobSetting, int>
            {
                { Scp914KnobSetting.Rough, rough },
                { Scp914KnobSetting.Coarse, coarse },
                { Scp914KnobSetting.OneToOne, oneToOne },
                { Scp914KnobSetting.Fine, fine },
                { Scp914KnobSetting.VeryFine, veryFine },
            };
        }

        /// <summary>
        /// Gets or sets the chances for each knob setting to succeed.
        /// </summary>
        public Dictionary<Scp914KnobSetting, int> Chances
        {
            get => chances;
            set
            {
                for (int i = 0; i < value.Count; i++)
                {
                    KeyValuePair<Scp914KnobSetting, int> kvp = value.ElementAt(i);
                    if (kvp.Value > 100)
                        value[kvp.Key] = 100;
                    else if (kvp.Value < 0)
                        value[kvp.Key] = 0;
                }

                chances = value;
            }
        }

        /// <summary>
        /// Compares the knobs chance against a random number to see if the upgrade chance passed.
        /// </summary>
        /// <param name="scp914KnobSetting">The setting to check.</param>
        /// <returns>A value indicating whether the check passed.</returns>
        public bool Check(Scp914KnobSetting scp914KnobSetting) => Chances.TryGetValue(scp914KnobSetting, out int chance) && Exiled.Loader.Loader.Random.Next(100) < chance;
    }
}