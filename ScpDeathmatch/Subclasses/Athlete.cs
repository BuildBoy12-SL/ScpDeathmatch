// -----------------------------------------------------------------------
// <copyright file="Athlete.cs" company="Build">
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
    using PlayerStatsSystem;
    using ScpDeathmatch.Subclasses.Abilities;

    /// <inheritdoc />
    public class Athlete : Subclass
    {
        private readonly Dictionary<Player, byte> previousIntensities = new();

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 90;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Athlete);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override string Badge { get; set; } = nameof(Athlete);

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

        /// <summary>
        /// Gets or sets the amount of cola a player has paired with the associated additional maximum health.
        /// </summary>
        [Description("The amount of cola a player has paired with the associated additional maximum health.")]
        public SortedList<byte, int> ColaHealth { get; set; } = new()
        {
            { 1, 5 },
            { 2, 5 },
            { 3, 5 },
            { 4, 5 },
        };

        /// <summary>
        /// Gets or sets a value indicating whether players should be immune to fall damage.
        /// </summary>
        [Description("Whether players should be immune to fall damage.")]
        public bool FallDamageImmunity { get; set; } = true;

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
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (Check(ev.Target))
                ev.Target.MaxHealth = MaxHealth;
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (FallDamageImmunity &&
                Check(ev.Target) &&
                ev.Handler.Base is UniversalDamageHandler universalDamageHandler &&
                universalDamageHandler.TranslationId == DeathTranslations.Falldown.Id)
                ev.IsAllowed = false;
        }

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!Check(ev.Player))
                return;

            byte newIntensity = ev.Player.GetEffectIntensity<Scp207>();
            if (previousIntensities.TryGetValue(ev.Player, out byte previousIntensity) && previousIntensity == newIntensity)
                return;

            previousIntensities[ev.Player] = newIntensity;
            if (ColaHealth.TryGetValue(newIntensity, out int additionalHealth))
                ev.Player.MaxHealth += additionalHealth;
        }
    }
}