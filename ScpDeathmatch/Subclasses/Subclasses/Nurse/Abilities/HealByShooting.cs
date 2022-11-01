// -----------------------------------------------------------------------
// <copyright file="HealByShooting.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Nurse.Abilities
{
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using InventorySystem.Items.Armor;
    using ScpDeathmatch.Models;
    using UnityEngine;

    /// <inheritdoc />
    public class HealByShooting : PassiveAbility
    {
        /// <inheritdoc />
        public override string Name { get; set; } = "HealByShooting";

        /// <inheritdoc />
        public override string Description { get; set; } = "Heals teammates when shot instead of damaging them";

        /// <summary>
        /// Gets or sets the hint to be displayed to those healed by the shot.
        /// </summary>
        public Hint Hint { get; set; } = new("You have been healed by {0}", 2);

        /// <summary>
        /// Gets or sets the value to multiply the damage by when calculating the heal amount.
        /// </summary>
        public float HealthMultiplier { get; set; } = 1f;

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shot += OnShot;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shot -= OnShot;
            base.UnsubscribeEvents();
        }

        private static int GetArmorEfficacy(Player player, HitboxType hitbox)
        {
            if (player.Inventory.TryGetBodyArmor(out var bodyArmor))
            {
                switch (hitbox)
                {
                    case HitboxType.Body:
                        return bodyArmor.VestEfficacy;
                    case HitboxType.Headshot:
                        return bodyArmor.HelmetEfficacy;
                }
            }

            return 0;
        }

        private void OnShot(ShotEventArgs ev)
        {
            if (ev.Shooter is null || ev.Target is null || ev.Shooter.Role.Side != ev.Target.Role.Side ||
                !ev.Shooter.IsSubclass<Nurse>() || ev.Shooter.CurrentItem is not Firearm firearm)
                return;

            int bulletPenetrationPercent = Mathf.RoundToInt(firearm.Base.ArmorPenetration * 100f);
            float num = (float)ev.Hitbox._dmgMultiplier / 100f;
            float toHealRaw = BodyArmorUtils.ProcessDamage(GetArmorEfficacy(ev.Target, ev.Hitbox._dmgMultiplier), ev.Damage, bulletPenetrationPercent) * num;
            float toHeal = toHealRaw * HealthMultiplier;

            ev.Target.Heal(toHeal);
            ev.CanHurt = false;
            Hint.DisplayFormatted(ev.Target, ev.Shooter.Nickname);
        }
    }
}