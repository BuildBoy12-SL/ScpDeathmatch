// -----------------------------------------------------------------------
// <copyright file="BigIron.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems
{
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Items;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using InventorySystem.Items.Firearms;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomItem(ItemType.GunRevolver)]
    public class BigIron : CustomWeapon
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 127;

        /// <inheritdoc />
        public override string Name { get; set; } = "Big Iron";

        /// <inheritdoc />
        public override string Description { get; set; } = "A revolver that can only hold one shot.";

        /// <inheritdoc />
        public override float Weight { get; set; }

        /// <inheritdoc />
        public override SpawnProperties SpawnProperties { get; set; } = new();

        /// <inheritdoc />
        public override float Damage { get; set; } = 100;

        /// <inheritdoc />
        public override byte ClipSize { get; set; } = 1;

        /// <summary>
        /// Gets or sets a value indicating whether the gun should be thrown when fired.
        /// </summary>
        [Description("Whether the gun should be thrown when fired.")]
        public bool ThrowGun { get; set; } = true;

        /// <inheritdoc />
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.GunRevolver;

        /// <inheritdoc />
        protected override void OnShooting(ShootingEventArgs ev)
        {
            if (ThrowGun)
            {
                if (ev.Shooter.CurrentItem is not Exiled.API.Features.Items.Firearm firearm)
                    return;

                Pickup pickup = Spawn(ev.Shooter.Position, (Player)null);
                if (pickup.Base is FirearmPickup firearmPickup)
                    firearmPickup.Status = new FirearmStatus(ClipSize, firearm.Base.Status.Flags, firearm.Base.Status.Attachments);

                ThrowItem(ev.Shooter, ev.Shooter.CurrentItem, pickup);
                ev.Shooter.RemoveHeldItem(true);
            }

            base.OnShooting(ev);
        }

        private static void ThrowItem(Player player, Item item, Pickup pickup)
        {
            if (pickup.Base.Rb is null)
                return;

            Vector3 vector = (player.ReferenceHub.playerMovementSync.PlayerVelocity / 3f) + (player.ReferenceHub.PlayerCameraReference.forward * 6f * (Mathf.Clamp01(Mathf.InverseLerp(7f, 0.1f, pickup.Base.Rb.mass)) + 0.3f));
            vector.x = Mathf.Max(Mathf.Abs(player.ReferenceHub.playerMovementSync.PlayerVelocity.x), Mathf.Abs(vector.x)) * ((vector.x < 0f) ? -1 : 1);
            vector.y = Mathf.Max(Mathf.Abs(player.ReferenceHub.playerMovementSync.PlayerVelocity.y), Mathf.Abs(vector.y)) * ((vector.y < 0f) ? -1 : 1);
            vector.z = Mathf.Max(Mathf.Abs(player.ReferenceHub.playerMovementSync.PlayerVelocity.z), Mathf.Abs(vector.z)) * ((vector.z < 0f) ? -1 : 1);
            pickup.Base.Rb.position = player.ReferenceHub.PlayerCameraReference.position;
            pickup.Base.Rb.velocity = vector;
            pickup.Base.Rb.angularVelocity = Vector3.Lerp(item.Base.ThrowSettings.RandomTorqueA, item.Base.ThrowSettings.RandomTorqueB, Random.value);
            float magnitude = pickup.Base.Rb.angularVelocity.magnitude;
            if (magnitude > pickup.Base.Rb.maxAngularVelocity)
                pickup.Base.Rb.maxAngularVelocity = magnitude;
        }
    }
}