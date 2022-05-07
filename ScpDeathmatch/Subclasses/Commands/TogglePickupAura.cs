// -----------------------------------------------------------------------
// <copyright file="TogglePickupAura.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Commands
{
    using System;
    using System.ComponentModel;
    using CommandSystem;
    using Exiled.API.Features;

    /// <inheritdoc />
    public class TogglePickupAura : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "togglepickupaura";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "paura", "taura" };

        /// <inheritdoc />
        public string Description { get; set; } = "Toggles the pickup aura.";

        /// <summary>
        /// Gets or sets the response to send when the player is not a scavenger.
        /// </summary>
        [Description("The response to send when the player is not a scavenger.")]
        public string NotScavengerResponse { get; set; } = "You are not a Scavenger!";

        /// <summary>
        /// Gets or sets the response to send when the player activates the scavenger aura.
        /// </summary>
        [Description("The response to send when the player activates the scavenger aura.")]
        public string ActivatedAura { get; set; } = "The Scavenger aura is now active.";

        /// <summary>
        /// Gets or sets the response to send when the player pauses the scavenger aura.
        /// </summary>
        [Description("The response to send when the player pauses the scavenger aura.")]
        public string PausedAura { get; set; } = "The Scavenger aura is now paused.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Player.Get(sender) is not Player player)
            {
                response = "You need to be a player to execute this command!";
                return false;
            }

            if (!Plugin.Instance.Config.Subclasses.Scavenger.Check(player))
            {
                response = NotScavengerResponse;
                return false;
            }

            if (player.SessionVariables.ContainsKey("PauseScavengerAura"))
            {
                player.SessionVariables.Remove("PauseScavengerAura");
                response = ActivatedAura;
                return true;
            }

            player.SessionVariables.Add("PauseScavengerAura", true);
            response = PausedAura;
            return true;
        }
    }
}