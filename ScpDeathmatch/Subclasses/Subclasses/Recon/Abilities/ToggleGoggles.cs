// -----------------------------------------------------------------------
// <copyright file="ToggleGoggles.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Recon.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using ScpDeathmatch.Subclasses.Interfaces;

    /// <inheritdoc cref="PassiveAbility"/>
    public class ToggleGoggles : PassiveAbility, IToggleablePassiveAbility
    {
        private readonly List<Player> activeList = new();

        /// <inheritdoc />
        public override string Name { get; set; } = "Noise Goggles";

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of 939 visuals.
        /// </summary>
        [Description("The type of 939 visuals.")]
        public byte Intensity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum distance that other players can be seen.
        /// </summary>
        [Description("The maximum distance that other players can be seen.")]
        public float MaximumDistance { get; set; } = 40f;

        /// <summary>
        /// Gets or sets the response to send when the player activates the recon goggles.
        /// </summary>
        [Description("The response to send when the player activates the recon goggles.")]
        public string ActivatedGoggles { get; set; } = "The goggles are now active.";

        /// <summary>
        /// Gets or sets the response to send when the player pauses the recon goggles.
        /// </summary>
        [Description("The response to send when the player disables the recon goggles.")]
        public string DisabledGoggles { get; set; } = "The goggles are now disabled.";

        /// <inheritdoc />
        public bool Toggle(Player player, out string response)
        {
            if (activeList.Contains(player))
            {
                Disable(player);
                response = DisabledGoggles;
                return true;
            }

            Enable(player);
            response = ActivatedGoggles;
            return true;
        }

        private void Disable(Player player)
        {
            player.DisableEffect(EffectType.Visuals939);
            activeList.Remove(player);
        }

        private void Enable(Player player)
        {
            player.EnableEffect(EffectType.Visuals939);
            player.ChangeEffectIntensity(EffectType.Visuals939, Intensity);
            activeList.Add(player);
        }
    }
}