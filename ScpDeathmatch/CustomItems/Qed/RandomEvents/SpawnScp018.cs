// -----------------------------------------------------------------------
// <copyright file="SpawnScp018.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using System.ComponentModel;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using Footprinting;
    using InventorySystem.Items.Pickups;
    using InventorySystem.Items.ThrowableProjectiles;
    using Mirror;
    using Respawning;
    using ScpDeathmatch.CustomItems.Qed.Enums;
    using UnityEngine;

    /// <inheritdoc />
    public class SpawnScp018 : IRandomEvent
    {
        /// <inheritdoc />
        public string Name { get; set; } = nameof(SpawnScp018);

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public RangeType Range { get; set; } = RangeType.Far;

        /// <summary>
        /// Gets or sets the amount of balls to spawn.
        /// </summary>
        [Description("The amount of balls to spawn.")]
        public int Amount { get; set; } = 1;

        /// <inheritdoc />
        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            ExplosiveGrenade explosiveGrenade = (ExplosiveGrenade)Item.Create(ItemType.SCP018, ev.Thrower);
            for (int i = 0; i < Amount; i++)
                Throw(explosiveGrenade.Base, ev.Grenade.transform.position);
        }

        private static void Throw(ThrowableItem throwableItem, Vector3 position)
        {
            Vector3 startVel = ThrowableNetworkHandler.GetLimitedVelocity(throwableItem.Owner.playerMovementSync.PlayerVelocity);
            ThrowableItem.ProjectileSettings projectileSettings = throwableItem.FullThrowSettings;
            Throw(throwableItem, projectileSettings.StartVelocity, projectileSettings.UpwardsFactor, projectileSettings.StartTorque, startVel, position);
        }

        private static void Throw(ThrowableItem throwableItem, float forceAmount, float upwardFactor, Vector3 torque, Vector3 startVel, Vector3 position)
        {
            if (throwableItem._alreadyFired && !throwableItem.IsLocalPlayer)
                return;
            throwableItem._destroyTime = Time.timeSinceLevelLoad + throwableItem._postThrownAnimationTime;
            throwableItem._alreadyFired = true;
            GameplayTickets.Singleton.HandleItemTickets(throwableItem);
            ThrownProjectile thrownProjectile1 = Object.Instantiate(throwableItem.Projectile, position, throwableItem.Owner.PlayerCameraReference.rotation);
            PickupSyncInfo pickupSyncInfo1 = default(PickupSyncInfo);
            pickupSyncInfo1.ItemId = throwableItem.ItemTypeId;
            pickupSyncInfo1.Locked = !throwableItem._repickupable;
            pickupSyncInfo1.Serial = throwableItem.ItemSerial;
            pickupSyncInfo1.Weight = throwableItem.Weight;
            pickupSyncInfo1.Position = thrownProjectile1.transform.position;
            pickupSyncInfo1.Rotation = new LowPrecisionQuaternion(thrownProjectile1.transform.rotation);
            PickupSyncInfo pickupSyncInfo2 = pickupSyncInfo1;
            thrownProjectile1.NetworkInfo = pickupSyncInfo2;
            thrownProjectile1.PreviousOwner = new Footprint(throwableItem.Owner);
            NetworkServer.Spawn(thrownProjectile1.gameObject);
            ThrownProjectile thrownProjectile2 = thrownProjectile1;
            pickupSyncInfo1 = default;
            PickupSyncInfo oldInfo = pickupSyncInfo1;
            PickupSyncInfo newInfo = pickupSyncInfo2;
            thrownProjectile2.InfoReceived(oldInfo, newInfo);
            if (thrownProjectile1.TryGetComponent(out Rigidbody component))
                throwableItem.PropelBody(component, torque, startVel, forceAmount, upwardFactor);
            thrownProjectile1.ServerActivate();
        }
    }
}