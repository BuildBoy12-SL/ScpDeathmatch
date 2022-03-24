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

    /// <summary>
    /// Handles configs related to custom items.
    /// </summary>
    public class CustomItemsConfig
    {
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.ReconSwitch"/> class.
        /// </summary>
        // public ReconSwitch ReconSwitch { get; set; } = new ReconSwitch();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.SecondWind"/> class.
        /// </summary>
        public SecondWind SecondWind { get; set; } = new SecondWind();

        /// <summary>
        /// Registers all custom items.
        /// </summary>
        public void Register()
        {
            // ReconSwitch.Register();
            SecondWind.Register();
        }

        /// <summary>
        /// Unregisters all custom items.
        /// </summary>
        public void Unregister()
        {
            // ReconSwitch.Unregister();
            SecondWind.Unregister();
        }
    }
}