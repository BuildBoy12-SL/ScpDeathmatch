// -----------------------------------------------------------------------
// <copyright file="MiscCommandsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using RemoteAdmin;
    using ScpDeathmatch.Commands;

    /// <summary>
    /// Handles configs for commands that to not fit in any other config.
    /// </summary>
    public class MiscCommandsConfig
    {
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="SpawnObject"/> command.
        /// </summary>
        public SpawnObject SpawnObject { get; set; } = new();

        /// <summary>
        /// Registers all commands.
        /// </summary>
        public void Register()
        {
            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(SpawnObject);
        }

        /// <summary>
        /// Unregisters all commands.
        /// </summary>
        public void Unregister()
        {
            CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(SpawnObject);
        }
    }
}