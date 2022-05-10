// -----------------------------------------------------------------------
// <copyright file="ChangingMaxHealthEventArgs.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.API.Events.EventArgs
{
    using System;
    using Exiled.API.Features;

    /// <summary>
    /// Contains all information after a player's maximum health is changed.
    /// </summary>
    public class ChangingMaxHealthEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangingMaxHealthEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="newMaxHealth"><inheritdoc cref="NewMaxHealth"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public ChangingMaxHealthEventArgs(Player player, int newMaxHealth, bool isAllowed = true)
        {
            Player = player;
            NewMaxHealth = newMaxHealth;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player whos max health changed.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the new maximum health.
        /// </summary>
        public int NewMaxHealth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the max health should be changed.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}