// -----------------------------------------------------------------------
// <copyright file="Runner.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using CustomPlayerEffects;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using GameCore;
    using MEC;
    using ScpDeathmatch.Subclasses.Abilities;

    /// <inheritdoc />
    public class Runner : Subclass
    {
        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Runner);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override string Badge { get; set; } = nameof(Runner);

        /// <inheritdoc />
        public override string BadgeColor { get; set; } = "yellow";

        /// <summary>
        /// Gets or sets the multiplier for movement speed.
        /// </summary>
        [Description("The multiplier for movement speed.")]
        public float MovementMultiplier { get; set; } = 1.15f;

        /// <summary>
        /// Gets or sets the multiplier for stamina capacity.
        /// </summary>
        [Description("The multiplier for stamina capacity.")]
        public float StaminaMultiplier { get; set; } = 1.2f;

        /// <inheritdoc />
        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new Tantrum(),
        };

        /// <inheritdoc />
        protected override void RoleAdded(Player player)
        {
            player.Stamina.StaminaUse /= StaminaMultiplier;
            Timing.CallDelayed(1.5f, () =>
            {
                player.ChangeWalkingSpeed(MovementMultiplier);
                player.ChangeRunningSpeed(MovementMultiplier);
            });

            base.RoleAdded(player);
        }

        /// <inheritdoc />
        protected override void RoleRemoved(Player player)
        {
            player.Stamina.StaminaUse = ConfigFile.ServerConfig.GetFloat("stamina_balance_use", 0.05f);
            Timing.CallDelayed(1.5f, () =>
            {
                player.ChangeWalkingSpeed(1f);
                player.ChangeRunningSpeed(1f);
            });

            base.RoleRemoved(player);
        }

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ReceivingEffect += OnReceivingEffect;
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ReceivingEffect -= OnReceivingEffect;
        }

        private void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            if (Check(ev.Player) && ev.Effect is Stained)
                ev.IsAllowed = false;
        }
    }
}