// -----------------------------------------------------------------------
// <copyright file="PlayerInfo.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Stats.Models
{
    using LiteDB;

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
        /// <param name="id"><inheritdoc cref="Id"/></param>
        /// <param name="roundKills"><inheritdoc cref="RoundKills"/></param>
        /// <param name="kills"><inheritdoc cref="Kills"/></param>
        [BsonCtor]
        public PlayerInfo(string id, int roundKills = 0, int kills = 0)
        {
            Id = id;
            RoundKills = roundKills;
            Kills = kills;
        }

        /// <summary>
        /// Gets the id of the player.
        /// </summary>
        public string Id { get; }

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