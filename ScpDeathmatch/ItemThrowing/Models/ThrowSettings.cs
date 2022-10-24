// -----------------------------------------------------------------------
// <copyright file="ThrowSettings.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.ItemThrowing.Models
{
    /// <summary>
    /// Represents the settings available to apply to a thrown item.
    /// </summary>
    public class ThrowSettings
    {
        /// <summary>
        /// Gets or sets the settings for items thrown at friendly players.
        /// </summary>
        public FriendlySettings FriendlySettings { get; set; }

        /// <summary>
        /// Gets or sets the settings for items thrown at enemy players.
        /// </summary>
        public EnemySettings EnemySettings { get; set; }
    }
}