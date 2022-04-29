// -----------------------------------------------------------------------
// <copyright file="CustomItemsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using Exiled.CustomItems.API.Features;
    using ScpDeathmatch.CustomItems;
    using ScpDeathmatch.CustomItems.Qed;

    /// <summary>
    /// Handles configs related to custom items.
    /// </summary>
    public class CustomItemsConfig
    {
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.ColaCoin"/> class.
        /// </summary>
        public ColaCoin ColaCoin { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.JammingCoin"/> class.
        /// </summary>
        public JammingCoin JammingCoin { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.Qed.Qed"/> class.
        /// </summary>
        public Qed Qed { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.ReconSwitch"/> class.
        /// </summary>
        public ReconSwitch ReconSwitch { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.SecondWind"/> class.
        /// </summary>
        public SecondWind SecondWind { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.StubbyRevolver"/> class.
        /// </summary>
        public StubbyRevolver StubbyRevolver { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.WeaponToken"/> class.
        /// </summary>
        public WeaponToken WeaponToken { get; set; } = new();

        /// <summary>
        /// Registers all custom items.
        /// </summary>
        public void Register() => CustomItem.RegisterItems(overrideClass: this);

        /// <summary>
        /// Unregisters all custom items.
        /// </summary>
        public void Unregister() => CustomItem.UnregisterItems();
    }
}