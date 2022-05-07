// -----------------------------------------------------------------------
// <copyright file="StickyJamming.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Abilities
{
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class StickyJamming : ActiveAbility
    {
        /// <inheritdoc />
        public override string Name { get; set; } = "Sticky Jamming";

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        [YamlIgnore]
        public override float Duration { get; set; }

        /// <inheritdoc />
        public override float Cooldown { get; set; } = 90f;

        /// <summary>
        /// Gets or sets the duration of the jam.
        /// </summary>
        [Description("The duration of the jam.")]
        public float JamDuration { get; set; } = 10f;

        /// <inheritdoc />
        public override bool CanUseAbility(Player player, out string response)
        {
            if (player.SessionVariables.ContainsKey("StickyJamming"))
            {
                response = "You already have a sticky coin queued!";
                return false;
            }

            return base.CanUseAbility(player, out response);
        }

        /// <inheritdoc />
        protected override void AbilityUsed(Player player)
        {
            player.SessionVariables.Add("StickyJamming", this);
        }

        /// <inheritdoc />
        protected override void AbilityEnded(Player player)
        {
            player.SessionVariables.Remove("StickyJamming");
        }
    }
}