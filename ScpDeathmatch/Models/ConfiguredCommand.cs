// -----------------------------------------------------------------------
// <copyright file="ConfiguredCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using MEC;

    /// <summary>
    /// Represents a command with a configurable delay in execution.
    /// </summary>
    public class ConfiguredCommand
    {
        /// <summary>
        /// Gets or sets the command to execute.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the delay before the command should be executed.
        /// </summary>
        public float Delay { get; set; }

        /// <summary>
        /// Executes the <see cref="Command"/> while respecting the <see cref="Delay"/>.
        /// </summary>
        /// <returns>The initialized coroutine.</returns>
        public CoroutineHandle Execute() => Timing.CallDelayed(Delay, () => GameCore.Console.singleton.TypeCommand(Command));
    }
}