// -----------------------------------------------------------------------
// <copyright file="GeneratorsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;
    using ScpDeathmatch.API.Interfaces;

    /// <summary>
    /// Handles configs related to generators.
    /// </summary>
    public class GeneratorsConfig : IConfigFile
    {
        /// <summary>
        /// Gets or sets the amount of time a generator will take to activate.
        /// </summary>
        [Description("The amount of time a generator will take to activate.")]
        public short DefaultTime { get; set; } = 60;

        /// <summary>
        /// Gets or sets the delay from when the round starts to when generators can be unlocked.
        /// </summary>
        [Description("The delay from when the round starts to when generators can be unlocked.")]
        public float UnlockDelay { get; set; } = 60f;

        /// <summary>
        /// Gets or sets the message to play when a generator is activated.
        /// </summary>
        [Description("The message to play when a generator is activated.")]
        public string EngagedAnnouncement { get; set; } = "{0} out of {1} generators activated";

        /// <summary>
        /// Gets or sets the message to play when all generators have been activated.
        /// </summary>
        [Description("The message to play when all generators have been activated.")]
        public string AllEngagedAnnouncement { get; set; } = "All generators have been successfully engaged";
    }
}