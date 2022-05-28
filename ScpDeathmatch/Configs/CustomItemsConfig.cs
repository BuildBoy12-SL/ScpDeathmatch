// -----------------------------------------------------------------------
// <copyright file="CustomItemsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using AdvancedHints.Enums;
    using Exiled.CustomItems.API;
    using Exiled.CustomItems.API.Features;
    using ScpDeathmatch.CustomItems;
    using ScpDeathmatch.CustomItems.Qed;
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Subclasses.Items;

    /// <summary>
    /// Handles configs related to custom items.
    /// </summary>
    public class CustomItemsConfig
    {
        private IEnumerable<CustomItem> registeredItems;

        /// <summary>
        /// Gets or sets the message to display to a player that is looking at a custom item.
        /// </summary>
        public Hint LookingAtMessage { get; set; } = new("You are looking at a {0}", 2f, true, DisplayLocation.Top);

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.BigIron"/> class.
        /// </summary>
        public BigIron BigIron { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Items.ColaCoin"/> class.
        /// </summary>
        public ColaCoin ColaCoin { get; set; } = new();

        /// <summary>
        /// Gets or sets the configurable instances of the <see cref="CustomItems.PunishmentGun"/> class.
        /// </summary>
        public List<PunishmentGun> PunishmentGuns { get; set; } = new()
        {
            new PunishmentGun { Id = 129, BanReason = "Rule 1 violation" },
            new PunishmentGun { Id = 130, BanReason = "Rule 2 violation" },
            new PunishmentGun { Id = 131, BanReason = "Rule 4 violation" },
        };

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.Qed.Qed"/> class.
        /// </summary>
        public Qed Qed { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.SecondWind"/> class.
        /// </summary>
        public SecondWind SecondWind { get; set; } = new();

        /// <summary>
        /// Registers all custom items.
        /// </summary>
        public void Register() => registeredItems = CustomItem.RegisterItems(overrideClass: this);

        /// <summary>
        /// Unregisters all custom items.
        /// </summary>
        public void Unregister()
        {
            if (registeredItems is null)
                return;

            foreach (CustomItem customItem in registeredItems)
                customItem.Unregister();
        }
    }
}