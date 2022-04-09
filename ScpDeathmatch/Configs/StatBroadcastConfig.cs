// -----------------------------------------------------------------------
// <copyright file="StatBroadcastConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;
    using Exiled.API.Features;

    /// <summary>
    /// Handles configs for the end of round statistic broadcast.
    /// </summary>
    public class StatBroadcastConfig
    {
        /// <summary>
        /// Gets or sets the broadcast to be sent at the end of the round.
        /// </summary>
        [Description("The broadcast to be sent at the end of the round. Variables: $Winner, $TopKills, $FirstBlood")]
        public Broadcast Broadcast { get; set; } = new("$Winner\n$TopKills\n$FirstBlood");

        /// <summary>
        /// Gets or sets the translation for the $FirstBlood variable in <see cref="Broadcast"/>.
        /// </summary>
        [Description("The translation for the $Winner variable in the broadcast.")]
        public string Winner { get; set; } = "Game Over! {0} has won!";

        /// <summary>
        /// Gets or sets the translation for the $FirstBlood variable in <see cref="Broadcast"/>.
        /// </summary>
        [Description("The translation for the $TopKills variable in the broadcast.")]
        public string TopKills { get; set; } = "{0} had the most kills at {1}";

        /// <summary>
        /// Gets or sets the translation for the $FirstBlood variable in <see cref="Broadcast"/>.
        /// </summary>
        [Description("The translation for the $FirstBlood variable in the broadcast.")]
        public string FirstBlood { get; set; } = "{0} got the first kill";
    }
}