// -----------------------------------------------------------------------
// <copyright file="ConsoleCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    /// <summary>
    /// Represents a custom command to display a message in the console when executed.
    /// </summary>
    public class ConsoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleCommand"/> class.
        /// </summary>
        public ConsoleCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleCommand"/> class.
        /// </summary>
        /// <param name="command"><inheritdoc cref="Command"/></param>
        /// <param name="response"><inheritdoc cref="Response"/></param>
        /// <param name="color"><inheritdoc cref="Color"/></param>
        public ConsoleCommand(string command, string response, string color = "white")
        {
            Command = command;
            Response = response;
            Color = color;
        }

        /// <summary>
        /// Gets or sets the command to execute.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the response of the command.
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the color of the response text.
        /// </summary>
        public string Color { get; set; }
    }
}