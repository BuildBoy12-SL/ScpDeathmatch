// -----------------------------------------------------------------------
// <copyright file="MedicalItemsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using ScpDeathmatch.HealthSystem.Models;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles the configs for modifying consumable medical items.
    /// </summary>
    public class MedicalItemsConfig
    {
        /// <summary>
        /// Gets or sets the actions to run when adrenaline is used.
        /// </summary>
        public MedicalActions Adrenaline { get; set; } = new()
        {
            AddedEffects = new List<ConfiguredEffect>
            {
                new(EffectType.MovementBoost, 20, 8f, true),
                new(EffectType.Invigorated, 1, 8f, true),
            },
            Ahp = new ConfiguredAhp(0f),
        };

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
        /// Gets or sets a value indicating whether Scp1853's stamina penalty will be ignored.
        /// </summary>
        [Description("Whether Scp1853's stamina penalty will be ignored.")]
        public bool Scp1853StaminaImmune { get; set; } = true;

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

        /// <summary>
        /// Gets or sets a list of all effects to activate when Scp500 is consumed.
        /// </summary>
        [Description("A list of all effects to activate when Scp500 is consumed.")]
        public List<ConfiguredEffect> Scp500Effects { get; set; } = new()
        {
            new ConfiguredEffect(EffectType.MovementBoost, 50, 10f),
        };

        /// <summary>
        /// Gets or sets a value indicating whether using Scp500 will disarm the player.
        /// </summary>
        public bool Scp500DisarmUser { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating how long to disarm the player for.
        /// </summary>
        public float Scp500DisarmDuration { get; set; } = 10f;
    }
}