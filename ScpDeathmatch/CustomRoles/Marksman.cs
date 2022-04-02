// -----------------------------------------------------------------------
// <copyright file="Marksman.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomRole(RoleType.ClassD)]
    public class Marksman : CustomRole
    {
        private float staminaOnKill = 20f;

        /// <inheritdoc />
        public override uint Id { get; set; } = 103;

        /// <inheritdoc />
        public override RoleType Role { get; set; } = RoleType.ClassD;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Marksman);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override Vector3 Scale { get; set; } = Vector3.one;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; }

        /// <inheritdoc />
        [YamlIgnore]
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();

        /// <inheritdoc />
        [YamlIgnore]
        public override bool RemovalKillsPlayer { get; set; } = false;

        /// <inheritdoc />
        [YamlIgnore]
        public override bool KeepRoleOnDeath { get; set; } = true;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<string> Inventory { get; set; } = new List<string>();

        /// <inheritdoc />
        [YamlIgnore]
        public override bool KeepInventoryOnSpawn { get; set; } = true;

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
        /// Gets or sets the multiplier for damage dealt with guns.
        /// </summary>
        [Description("The multiplier for damage dealt with guns.")]
        public float DamageMultiplier { get; set; } = 1.25f;

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
            if (ev.Killer == null || !Check(ev.Killer))
                return;

            ev.Killer.Heal(HealthOnKill);
            ev.Killer.Stamina.RemainingStamina = Mathf.Clamp(ev.Killer.Stamina.RemainingStamina + (StaminaOnKill / 100f), 0f, 1f);
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker != null && Check(ev.Attacker))
                ev.Amount *= DamageMultiplier;
        }
    }
}