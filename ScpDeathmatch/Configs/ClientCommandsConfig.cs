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
        private readonly List<ICommand> registeredCommands = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.Client.Remove1853"/> class.
        /// </summary>
        public Remove1853 Remove1853 { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Commands.Client.Remove207"/> class.
        /// </summary>
        public Remove207 Remove207 { get; set; } = new();

        /// <summary>
        /// Gets or sets custom client commands.
        /// </summary>
        [Description("Custom client commands.")]
        public List<ConsoleCommand> CustomCommands { get; set; } = new()
        {
            new ConsoleCommand("info207", "207 has been tweaked on out server to not do damage!"),
        };

        /// <summary>
        /// Gets or sets client commands that are directly related to subclass functionality.
        /// </summary>
        public SubclassCommandsConfig SubclassCommands { get; set; } = new();

        /// <summary>
        /// Registers all commands.
        /// </summary>
        public void Register()
        {
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                if (property.GetValue(this) is ICommand command)
                {
                    QueryProcessor.DotCommandHandler.RegisterCommand(command);
                    registeredCommands.Add(command);
                }
            }

            SubclassCommands.Register();
        }

        /// <summary>
        /// Unregisters all commands.
        /// </summary>
        public void Unregister()
        {
            foreach (ICommand command in registeredCommands)
                QueryProcessor.DotCommandHandler.UnregisterCommand(command);

            registeredCommands.Clear();
            SubclassCommands.Unregister();
        }

        /// <summary>
        /// Handles commands to be executed by subclasses.
        /// </summary>
        public class SubclassCommandsConfig
        {
            private readonly List<ICommand> registeredCommands = new();

            /// <summary>
            /// Gets or sets a configurable instance of the <see cref="Subclasses.Commands.ToggleGoggles"/> class.
            /// </summary>
            public ToggleGoggles ToggleGoggles { get; set; } = new();

            /// <summary>
            /// Gets or sets a configurable instance of the <see cref="Subclasses.Commands.TogglePickupAura"/> class.
            /// </summary>
            public TogglePickupAura TogglePickupAura { get; set; } = new();

            /// <summary>
            /// Registers all commands.
            /// </summary>
            public void Register()
            {
                foreach (PropertyInfo property in GetType().GetProperties())
                {
                    if (property.GetValue(this) is ICommand command)
                    {
                        QueryProcessor.DotCommandHandler.RegisterCommand(command);
                        registeredCommands.Add(command);
                    }
                }
            }

            /// <summary>
            /// Unregisters all commands.
            /// </summary>
            public void Unregister()
            {
                foreach (ICommand command in registeredCommands)
                    QueryProcessor.DotCommandHandler.UnregisterCommand(command);

                registeredCommands.Clear();
            }
        }
    }
}