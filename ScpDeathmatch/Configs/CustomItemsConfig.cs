// -----------------------------------------------------------------------
// <copyright file="CustomItemsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using Exiled.CustomItems.API;
    using ScpDeathmatch.CustomItems;
    using ScpDeathmatch.CustomItems.Qed;

    /// <summary>
    /// Handles configs related to custom items.
    /// </summary>
    public class CustomItemsConfig
    {
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.Qed.Qed"/> class.
        /// </summary>
        public Qed Qed { get; set; } = new Qed();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.ReconSwitch"/> class.
        /// </summary>
        public ReconSwitch ReconSwitch { get; set; } = new ReconSwitch();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.SecondWind"/> class.
        /// </summary>
        public SecondWind SecondWind { get; set; } = new SecondWind();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.WeaponToken"/> class.
        /// </summary>
        public WeaponToken WeaponToken { get; set; } = new WeaponToken();

        /// <summary>
        /// Registers all custom items.
        /// </summary>
        public void Register()
        {
            Qed.Register();
            ReconSwitch.Register();
            SecondWind.Register();
            WeaponToken.Register();
        }

        /// <summary>
        /// Unregisters all custom items.
        /// </summary>
        public void Unregister()
        {
            Qed.Unregister();
            ReconSwitch.Unregister();
            SecondWind.Unregister();
            WeaponToken.Unregister();
        }
    }
}