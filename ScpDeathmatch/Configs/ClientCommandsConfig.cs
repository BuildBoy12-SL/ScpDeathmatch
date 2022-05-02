// -----------------------------------------------------------------------
// <copyright file="ClientCommandsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Reflection;
    using CommandSystem;
    using RemoteAdmin;
    using ScpDeathmatch.Commands.Client;

    /// <summary>
    /// Handles configs related to client commands.
    /// </summary>
    public class ClientCommandsConfig
    {
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.Client.Remove1853"/> class.
        /// </summary>
        public Remove1853 Remove1853 { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.Client.Remove207"/> class.
        /// </summary>
        public Remove207 Remove207 { get; set; } = new();

        /// <summary>
        /// Registers all commands.
        /// </summary>
        public void Register()
        {
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                if (property.GetValue(this) is ICommand command)
                    QueryProcessor.DotCommandHandler.RegisterCommand(command);
            }
        }

        /// <summary>
        /// Unregisters all commands.
        /// </summary>
        public void Unregister()
        {
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                if (property.GetValue(this) is ICommand command)
                    QueryProcessor.DotCommandHandler.UnregisterCommand(command);
            }
        }
    }
}