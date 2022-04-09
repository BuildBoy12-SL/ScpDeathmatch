// -----------------------------------------------------------------------
// <copyright file="PlayerInfo.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Stats.Models
{
    /// <summary>
    /// Represents a player's information.
    /// </summary>
    public class PlayerInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerInfo"/> class.
        /// </summary>
        public PlayerInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerInfo"/> class.
        /// </summary>
        /// <param name="userId"><inheritdoc cref="UserId"/></param>
        public PlayerInfo(string userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Gets or sets the user id of the player.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the amount of kills the player has gotten in the current round.
        /// </summary>
        public int RoundKills { get; set; }

        /// <summary>
        /// Gets or sets the total kills of the player.
        /// </summary>
        public int Kills { get; set; }

        /// <summary>
        /// Gets or sets the total deaths of the player.
        /// </summary>
        public int Deaths { get; set; }
    }
}