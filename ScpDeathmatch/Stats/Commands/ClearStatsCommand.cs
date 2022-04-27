// -----------------------------------------------------------------------
// <copyright file="ClearStatsCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Stats.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using ScpDeathmatch.Stats.Models;

    /// <inheritdoc />
    public class ClearStatsCommand : ICommand
    {
        /// <inheritdoc />
        public string Command => "clearstats";

        /// <inheritdoc />
        public string[] Aliases { get; } = Array.Empty<string>();

        /// <inheritdoc />
        public string Description => "Clears the stats of a player.";

        /// <summary>
        /// Gets or sets the permission required to execute this command.
        /// </summary>
        public string RequiredPermission { get; set; } = "sd.clearstats";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = "You don't have permission to execute this command.";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: clearstats <player/userId>";
                return false;
            }

            Player player = Player.Get(arguments.At(0));
            if (!Plugin.Instance.StatDatabase.TryGet(player, out PlayerInfo playerInfo) &&
                !Plugin.Instance.StatDatabase.TryGet(arguments.At(0), out playerInfo))
            {
                response = "Unable to find a player with the specified info in the database.";
                return false;
            }

            Plugin.Instance.StatDatabase.Reset(playerInfo);
            response = $"Reset the statistics of {playerInfo.Id}.";
            return true;
        }
    }
}