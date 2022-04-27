// -----------------------------------------------------------------------
// <copyright file="ForceAnnouncerCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Commands
{
    using System;
    using CommandSystem;
    using Exiled.Permissions.Extensions;

    /// <inheritdoc />
    public class ForceAnnouncerCommand : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "forceannouncer";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "forceann", "fannouncer" };

        /// <inheritdoc />
        public string Description { get; set; } = "Forces the C.A.S.S.I.E announcer to announce how many players are left in each zone";

        /// <summary>
        /// Gets or sets the permission required to execute this command.
        /// </summary>
        public string RequiredPermission { get; set; } = "sd.announcer";

        /// <summary>
        /// Gets or sets the response to send to the sender when they lack the <see cref="RequiredPermission"/>.
        /// </summary>
        public string InsufficientPermissionResponse { get; set; } = "You do not have permission to use this command.";

        /// <summary>
        /// Gets or sets the response to send to the sender when the command is disabled in the config.
        /// </summary>
        public string AnnouncerDisabledResponse { get; set; } = "This command is currently disabled.";

        /// <summary>
        /// Gets or sets the response to send to the sender when the command executes successfully.
        /// </summary>
        public string SuccessResponse { get; set; } = "Announcer invoked.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = InsufficientPermissionResponse;
                return false;
            }

            if (!Plugin.Instance.Config.ZoneAnnouncer.IsEnabled)
            {
                response = AnnouncerDisabledResponse;
                return false;
            }

            Plugin.Instance.ZoneAnnouncer.Announce();
            response = SuccessResponse;
            return true;
        }
    }
}