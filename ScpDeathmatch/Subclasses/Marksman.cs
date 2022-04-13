// -----------------------------------------------------------------------
// <copyright file="Marksman.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.Models;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class Marksman : Subclass
    {
        private float staminaOnKill = 20f;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Marksman);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override string Badge { get; set; } = nameof(Marksman);

        /// <inheritdoc />
        public override string BadgeColor { get; set; } = "army_green";

        /// <summary>
        /// Gets or sets the amount of health received on kill.
        /// </summary>
        [Description("The amount of health received on kill.")]
        public float HealthOnKill { get; set; } = 20f;

        /// <summary>
        /// Gets or sets the amount of stamina received on kill as a percentage.
        /// </summary>
        [Description("The amount of stamina received on kill as a percentage.")]
        public float StaminaOnKill
        {
            get => staminaOnKill;
            set => staminaOnKill = Mathf.Clamp(value, 0f, 100f);
        }

        /// <summary>
        /// Gets or sets the effects to apply on kill.
        /// </summary>
        [Description("The effects to apply on kill.")]
        public List<ConfiguredEffect> EffectsOnKill { get; set; } = new()
        {
            new ConfiguredEffect(EffectType.Invigorated, 1, 5f),
            new ConfiguredEffect(EffectType.MovementBoost, 20, 5f),
        };

        /// <summary>
        /// Gets or sets the multiplier for damage dealt with guns.
        /// </summary>
        [Description("The multiplier for damage dealt with guns.")]
        public float DamageMultiplier { get; set; } = 1.25f;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; } = new();

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnsubscribeEvents();
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (ev.Killer is null || !Check(ev.Killer))
                return;

            ev.Killer.Heal(HealthOnKill);
            ev.Killer.Stamina.RemainingStamina = Mathf.Clamp(ev.Killer.Stamina.RemainingStamina + (StaminaOnKill / 100f), 0f, 1f);
            foreach (ConfiguredEffect effect in EffectsOnKill)
                effect.Apply(ev.Killer);
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker is not null && Check(ev.Attacker))
                ev.Amount *= DamageMultiplier;
        }
    }
}