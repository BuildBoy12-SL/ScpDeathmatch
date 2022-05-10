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
    using Exiled.API.Extensions;
    using Exiled.API.Features.Items;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Subclasses.Abilities;
    using UnityEngine;

    /// <inheritdoc />
    public class Marksman : Subclass
    {
        private readonly List<int> grantedScp1853 = new();
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
        public override ConfiguredBadge Badge { get; set; } = new(nameof(Marksman), "army_green");

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
        /// Gets or sets a value indicating whether the player's maximum health should be restored on kill.
        /// </summary>
        [Description("Whether the player's maximum health should be restored on kill.")]
        public bool ResetMaxHealthOnKill { get; set; } = true;

        /// <summary>
        /// Gets or sets the multiplier for damage dealt with guns.
        /// </summary>
        [Description("The multiplier for damage dealt with guns.")]
        public float DamageMultiplier { get; set; } = 1f;

        /// <summary>
        /// Gets or sets a value indicating whether the player will be granted an <see cref="ItemType.SCP1853"/> when they pick up a gun for the first time.
        /// </summary>
        public bool GiveScp1853OnFirstWeapon { get; set; } = true;

        /// <inheritdoc />
        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new Cracked1853(),
        };

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingItem;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingItem;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
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

            if (ResetMaxHealthOnKill)
                ev.Killer.MaxHealth = ev.Killer.ReferenceHub.characterClassManager.CurRole.maxHP;
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker is not null && Check(ev.Attacker))
                ev.Amount *= DamageMultiplier;
        }

        private void OnPickingItem(PickingUpItemEventArgs ev)
        {
            if (!ev.IsAllowed || !GiveScp1853OnFirstWeapon || !Check(ev.Player) || grantedScp1853.Contains(ev.Player.Id) || !ev.Pickup.Type.IsWeapon())
                return;

            if (ev.Player.Items.Count >= 7)
                Item.Create(ItemType.SCP1853).Spawn(ev.Player.Position + Vector3.up);
            else
                ev.Player.AddItem(ItemType.SCP1853);

            grantedScp1853.Add(ev.Player.Id);
        }

        private void OnWaitingForPlayers() => grantedScp1853.Clear();
    }
}