// -----------------------------------------------------------------------
// <copyright file="Badge.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using Exiled.API.Features;

    /// <summary>
    /// Represents a rank badge.
    /// </summary>
    public class Badge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Badge"/> class.
        /// </summary>
        public Badge()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Badge"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc cref="Name"/></param>
        /// <param name="color"><inheritdoc cref="Color"/></param>
        public Badge(string name, string color)
        {
            Name = name;
            Color = color;
        }

        /// <summary>
        /// Gets or sets the text of the badge.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the color of the badge.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Applies the badge to a player.
        /// </summary>
        /// <param name="player">The player to add the badge to.</param>
        public void Apply(Player player)
        {
            player.ReferenceHub.serverRoles.Network_myText = Name;
            player.ReferenceHub.serverRoles.Network_myColor = Color;
        }
    }
}