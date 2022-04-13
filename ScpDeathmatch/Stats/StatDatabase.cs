// -----------------------------------------------------------------------
// <copyright file="StatDatabase.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Stats
{
    using System.IO;
    using Exiled.API.Features;
    using LiteDB;
    using ScpDeathmatch.Stats.Models;

    /// <summary>
    /// Manages the database.
    /// </summary>
    public class StatDatabase
    {
        private readonly Plugin plugin;
        private LiteDatabase database;
        private ILiteCollection<PlayerInfo> playerInfoCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatDatabase"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public StatDatabase(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Initializes and opens the database.
        /// </summary>
        public void Open()
        {
            if (!Directory.Exists(plugin.Config.StatsDatabase.DirectoryPath))
                Directory.CreateDirectory(plugin.Config.StatsDatabase.DirectoryPath);

            database = new LiteDatabase(Path.Combine(plugin.Config.StatsDatabase.DirectoryPath, plugin.Config.StatsDatabase.FileName));
            playerInfoCollection = database.GetCollection<PlayerInfo>();
            playerInfoCollection.EnsureIndex(playerInfo => playerInfo.Id, true);
        }

        /// <summary>
        /// Closes and disposes of the database.
        /// </summary>
        public void Close()
        {
            database?.Checkpoint();
            database?.Dispose();
            database = null;
        }

        /// <summary>
        /// Gets the player info for the specified player.
        /// </summary>
        /// <param name="player">The player to find the info for.</param>
        /// <param name="playerInfo">The found <see cref="PlayerInfo"/>, or null if none is found.</param>
        /// <returns>Whether a matching <see cref="PlayerInfo"/> was successfully found.</returns>
        public bool TryGet(Player player, out PlayerInfo playerInfo) => TryGet(player.UserId, out playerInfo);

        /// <summary>
        /// Gets the player info for the specified user id.
        /// </summary>
        /// <param name="userId">The id to find the info for.</param>
        /// <param name="playerInfo">The found <see cref="PlayerInfo"/>, or null if none is found.</param>
        /// <returns>Whether a matching <see cref="PlayerInfo"/> was successfully found.</returns>
        public bool TryGet(string userId, out PlayerInfo playerInfo)
        {
            playerInfo = playerInfoCollection.FindOne(x => x.Id == userId);
            return playerInfo is not null;
        }

        /// <inheritdoc cref="ILiteCollection{T}.Upsert(T)"/>
        public void Upsert(PlayerInfo playerInfo) => playerInfoCollection.Upsert(playerInfo);
    }
}