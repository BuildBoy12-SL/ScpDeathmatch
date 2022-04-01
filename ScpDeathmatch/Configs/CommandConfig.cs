// -----------------------------------------------------------------------
// <copyright file="CommandConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles configs related to automated command execution.
    /// </summary>
    public class CommandConfig
    {
        /// <summary>
        /// Gets or sets the commands to be ran at the start of the round.
        /// </summary>
        [Description("The commands to be ran at the start of the round.")]
        public List<ConfiguredCommand> RoundStart { get; set; } = new List<ConfiguredCommand>
        {
            new ConfiguredCommand
            {
                Command = "/command1",
                Delay = 0f,
            },
            new ConfiguredCommand
            {
                Command = "/command2",
                Delay = 5f,
            },
        };

        /// <summary>
        /// Gets or sets the commands to be ran when the round ends.
        /// </summary>
        [Description("The commands to be ran when the round ends.")]
        public List<ConfiguredCommand> RoundEnd { get; set; } = new List<ConfiguredCommand>
        {
            new ConfiguredCommand
            {
                Command = "/command1",
                Delay = 0f,
            },
            new ConfiguredCommand
            {
                Command = "/command2",
                Delay = 5f,
            },
        };
    }
}