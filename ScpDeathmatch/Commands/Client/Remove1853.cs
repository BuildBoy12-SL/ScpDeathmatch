// -----------------------------------------------------------------------
// <copyright file="Remove1853.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Commands.Client
{
    using System;
    using System.ComponentModel;
    using CommandSystem;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.API.Features;

    /// <inheritdoc />
    public class Remove1853 : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "remove1853";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "r1853" };

        /// <inheritdoc />
        public string Description { get; set; } = "Removes the Scp1853 status effect from the user.";

        /// <summary>
        /// Gets or sets the response to send the player when they do not have the Scp-207 status effect.
        /// </summary>
        [Description("The message to send the player when they do not have the Scp-1853 status effect.")]
        public string Scp1853NotEnabledResponse { get; set; } = "You do not have the Scp-1853 status effect.";

        /// <summary>
        /// Gets or sets the response to send the player when the command is executed successfully.
        /// </summary>
        [Description("The response to send the player when the command is executed successfully.")]
        public string SuccessResponse { get; set; } = "You have removed the Scp-1853 status effect from yourself.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Player.Get(sender) is not Player player)
            {
                response = "You must be a player to execute this command!";
                return false;
            }

            PlayerEffect scp1853 = player.GetEffect(EffectType.Scp1853);
            if (!scp1853.IsEnabled)
            {
                response = Scp1853NotEnabledResponse;
                return false;
            }

            scp1853.IsEnabled = false;
            response = SuccessResponse;
            return true;
        }
    }
}