// -----------------------------------------------------------------------
// <copyright file="DisarmingConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;
    using ScpDeathmatch.API.Interfaces;

    /// <summary>
    /// Handles the configs related to disarming.
    /// </summary>
    public class DisarmingConfig : IConfigFile
    {
        /// <summary>
        /// Gets or sets the amount of people a single person can detain.
        /// </summary>
        [Description("The amount of people a single person can detain.")]
        public int DetainLimit { get; set; } = 1;

        /// <summary>
        /// Gets or sets a value indicating whether players will drop their items when disarmed.
        /// </summary>
        [Description("Whether players will drop their items when disarmed.")]
        public bool DropItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether players will be automatically disarmed when they get too far away.
        /// </summary>
        [Description("Whether players will be automatically disarmed when they get too far away.")]
        public bool DisarmAtDistance { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether disarmed players will take damage from other players.
        /// </summary>
        [Description("Whether disarmed players will take damage from other players.")]
        public bool DisarmedDamage { get; set; } = false;
    }
}