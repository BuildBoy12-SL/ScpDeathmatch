// -----------------------------------------------------------------------
// <copyright file="EnhancedReconSwitch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Abilities
{
    using System.ComponentModel;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;

    /// <inheritdoc />
    public class EnhancedReconSwitch : ActiveAbility
    {
        /// <inheritdoc />
        public override string Name { get; set; } = "Enhanced Recon Switch";

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override float Duration { get; set; } = 10f;

        /// <inheritdoc />
        public override float Cooldown { get; set; } = 30f;

        /// <summary>
        /// Gets or sets the multiplier for the range of the goggles while the ability is active.
        /// </summary>
        [Description("The multiplier for the range of the goggles while the ability is active.")]
        public float RangeMultiplier { get; set; } = 2;

        /// <inheritdoc />
        public override bool CanUseAbility(Player player, out string response)
        {
            if (!player.GetEffectActive<Visuals939>())
            {
                response = "You do not have the recon switch visual effects activated.";
                return false;
            }

            return base.CanUseAbility(player, out response);
        }

        /// <inheritdoc />
        protected override void AbilityUsed(Player player)
        {
            player.SessionVariables.Add("EnhancedReconSwitch", this);
        }

        /// <inheritdoc />
        protected override void AbilityEnded(Player player)
        {
            player.SessionVariables.Remove("EnhancedReconSwitch");
        }
    }
}