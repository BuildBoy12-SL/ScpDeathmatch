// -----------------------------------------------------------------------
// <copyright file="ActivatingConsumableEffectsEventArgs.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.API.Events.EventArgs
{
    using System;
    using Exiled.API.Features;
    using InventorySystem.Items.Usables;

    /// <summary>
    /// Contains all information before a player consumes a <see cref="InventorySystem.Items.Usables.Consumable"/>.
    /// </summary>
    public class ActivatingConsumableEffectsEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivatingConsumableEffectsEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="consumable"><inheritdoc cref="Consumable"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public ActivatingConsumableEffectsEventArgs(Player player, Consumable consumable, bool isAllowed = true)
        {
            Player = player;
            Consumable = consumable;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player that used the <see cref="Consumable"/>.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the item that was used.
        /// </summary>
        public Consumable Consumable { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the effects should be applied to the player.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}