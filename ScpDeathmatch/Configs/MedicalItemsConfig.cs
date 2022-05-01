// -----------------------------------------------------------------------
// <copyright file="MedicalItemsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles the configs for modifying consumable medical items.
    /// </summary>
    public class MedicalItemsConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether adrenaline should keep its ahp.
        /// </summary>
        [Description("Whether adrenaline should keep its ahp.")]
        public bool AdrenalineAhp { get; set; } = false;

        /// <summary>
        /// Gets or sets the intensity of the movement speed effect applied by adrenaline.
        /// </summary>
        [Description("The intensity of the movement speed effect applied by adrenaline.")]
        public byte AdrenalineMovementBoost { get; set; } = 20;

        /// <summary>
        /// Gets or sets the duration of the movement boost effect given by adrenaline.
        /// </summary>
        [Description("The duration of the movement boost effect given by adrenaline.")]
        public float AdrenalineMovementBoostDuration { get; set; } = 8f;

        /// <summary>
        /// Gets or sets the duration of the invigorated effect given by adrenaline.
        /// </summary>
        [Description("The duration of the invigorated effect given by adrenaline.")]
        public float AdrenalineInvigoratedDuration { get; set; } = 8f;

        /// <summary>
        /// Gets or sets a value indicating whether painkillers should keep their regeneration.
        /// </summary>
        [Description("Whether painkillers should keep their regeneration.")]
        public bool PainkillersRegeneration { get; set; } = false;

        /// <summary>
        /// Gets or sets the ahp to grant players when they consume painkillers.
        /// </summary>
        [Description("The amount of ahp to grant players when they consume painkillers.")]
        public ConfiguredAhp PainkillersAhp { get; set; } = new(25f, sustain: 10f);

        /// <summary>
        /// Gets or sets a value indicating whether Scp500 should keep its regeneration.
        /// </summary>
        [Description("Whether Scp500 should keep its regeneration.")]
        public bool Scp500Regeneration { get; set; } = false;

        /// <summary>
        /// Gets or sets the ahp to grant players when they consume painkillers.
        /// </summary>
        [Description("The amount of ahp to grant players when they consume painkillers.")]
        public ConfiguredAhp Scp500Ahp { get; set; } = new(50f, sustain: 10f);

        /// <summary>
        /// Gets or sets a value indicating whether Scp500 will remove the Scp207 effect.
        /// </summary>
        [Description("Whether Scp500 will remove the Scp207 effect.")]
        public bool Scp500RemoveScp207 { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether Scp500 will remove the Scp1853 effect.
        /// </summary>
        [Description("Whether Scp500 will remove the Scp1853 effect.")]
        public bool Scp500RemoveScp1853 { get; set; } = false;
    }
}