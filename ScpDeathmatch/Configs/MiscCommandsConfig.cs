// -----------------------------------------------------------------------
// <copyright file="MiscCommandsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using RemoteAdmin;
    using ScpDeathmatch.API.Interfaces;
    using ScpDeathmatch.Commands;

    /// <summary>
    /// Handles configs for commands that to not fit in any other config.
    /// </summary>
    public class MiscCommandsConfig : IConfigFile
    {
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="ChangeSubclassCommand"/> command.
        /// </summary>
        public ChangeSubclassCommand ChangeSubclass { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="SpawnObjectCommand"/> command.
        /// </summary>
        public SpawnObjectCommand SpawnObject { get; set; } = new();

        /// <summary>
        /// Registers all commands.
        /// </summary>
        public void Register()
        {
            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(ChangeSubclass);
            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(SpawnObject);
        }

        /// <summary>
        /// Unregisters all commands.
        /// </summary>
        public void Unregister()
        {
            CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(ChangeSubclass);
            CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(SpawnObject);
        }
    }
}