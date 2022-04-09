// -----------------------------------------------------------------------
// <copyright file="ExtraLivesConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;
    using Exiled.API.Features;
    using ScpDeathmatch.Enums;

    /// <summary>
    /// Handles configs related to granting lives for players who have others disarmed.
    /// </summary>
    public class ExtraLivesConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether extra lives is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a player that has handcuffed somebody will respawn if the cause of death was a suicide.
        /// </summary>
        [Description("Whether a player that has handcuffed somebody will respawn if the cause of death was a suicide.")]
        public bool RespawnOnSuicide { get; set; } = true;

        /// <summary>
        /// Gets or sets the method of teleportation the cuffer will receive on death.
        /// </summary>
        [Description("The method of teleportation the cuffer will receive on death. Available: None, Zone, Role")]
        public TeleportType TeleportType { get; set; } = TeleportType.Zone;

        /// <summary>
        /// Gets or sets a value indicating whether players that receive an extra life will spawn a ragdoll upon 'death'.
        /// </summary>
        [Description("Whether players that receive an extra life will spawn a ragdoll upon 'death'.")]
        public bool SpawnRagdolls { get; set; } = true;

        /// <summary>
        /// Gets or sets the broadcast to send a player when they receive an extra life.
        /// </summary>
        [Description("The broadcast to send a player when they receive an extra life.")]
        public Broadcast ExtraLifeBroadcast { get; set; } = new("You have received an extra life at the cost of those you had detained!");
    }
}