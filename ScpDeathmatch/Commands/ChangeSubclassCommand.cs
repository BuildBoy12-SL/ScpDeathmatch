// -----------------------------------------------------------------------
// <copyright file="ChangeSubclassCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Commands
{
    using System;
    using System.ComponentModel;
    using CommandSystem;
    using Exiled.API.Features;
    using ScpDeathmatch.Subclasses;

    /// <inheritdoc />
    public class ChangeSubclassCommand : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "changesubclass";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "csubclass", "subclass" };

        /// <inheritdoc />
        public string Description { get; set; } = "Sets a player's subclass";

        /// <summary>
        /// Gets or sets the response to provide when an invalid usage is provided.
        /// </summary>
        [Description("The response to provide when an invalid usage is provided.")]
        public string UsageResponse { get; set; } = "Usage: changesubclass <subclass> [player]";

        /// <summary>
        /// Gets or sets the response to provide when the command is successfully executed.
        /// </summary>
        [Description("The response to provide when the command is successfully executed.")]
        public string SuccessResponse { get; set; } = "Set {0} ({1}) to the '{2}' subclass.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Player.Get(sender) is not Player player)
            {
                response = "This command can only be run by a player.";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = UsageResponse;
                return false;
            }

            if (Subclass.Get(arguments.At(0)) is not Subclass subclass)
            {
                response = $"{arguments.At(0)} is not a valid subclass.";
                return false;
            }

            if (arguments.Count > 1 && Player.Get(arguments.At(1)) is Player target)
                player = target;

            subclass.AddRole(player);
            response = string.Format(SuccessResponse, player.Nickname, player.Id, subclass.Name);
            return true;
        }
    }
}