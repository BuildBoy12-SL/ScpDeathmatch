// -----------------------------------------------------------------------
// <copyright file="ClientCommandsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using CommandSystem;
    using RemoteAdmin;
    using ScpDeathmatch.Commands.Client;
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Subclasses.Commands;

    /// <summary>
    /// Handles configs related to client commands.
    /// </summary>
    public class ClientCommandsConfig
    {
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.Client.Remove1853"/> class which is used to remove the Scp1853 effect from a player.
        /// </summary>
        [Description("Removes the Scp1853 effect from a player.")]
        public Remove1853 Remove1853 { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.Client.Remove207"/> class which is used to remove the Scp207 effect from a player.
        /// </summary>
        [Description("Removes the Scp207 effect from a player.")]
        public Remove207 Remove207 { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Commands.TogglePassive"/> class which allows a subclass to toggle their passive abilities.
        /// </summary>
        [Description("Allows a subclass to toggle their passive abilities.")]
        public TogglePassive TogglePassive { get; set; } = new();

        /// <summary>
        /// Gets or sets custom client commands.
        /// </summary>
        [Description("Custom client commands.")]
        public List<ConsoleCommand> CustomCommands { get; set; } = new()
        {
            new ConsoleCommand("info207", "207 has been tweaked on out server to not do damage!"),
        };

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