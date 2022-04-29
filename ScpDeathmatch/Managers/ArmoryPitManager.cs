// -----------------------------------------------------------------------
// <copyright file="ArmoryPitManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using UnityEngine;

    /// <summary>
    /// Manages the tracking of items thrown into the <see cref="RoomType.HczArmory"/> pit.
    /// </summary>
    public class ArmoryPitManager
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArmoryPitManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public ArmoryPitManager(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
        }

        private void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.CurrentRoom is null || ev.Player.CurrentRoom.Type != RoomType.HczArmory)
                return;

            ev.IsAllowed = false;
            ev.Player.RemoveItem(ev.Item);

            if (ev.Player.Inventory.UserInventory.Items.ContainsKey(ev.Item.Serial))
            {
                ev.Player.Inventory.UserInventory.Items.Remove(ev.Item.Serial);
                ev.Player.Inventory.SendItemsNextFrame = true;
            }

            Pickup pickup = CustomItem.TryGet(ev.Item, out CustomItem customItem) ? customItem.Spawn(ev.Player.Position, (Player)null) : ev.Item.Spawn(ev.Player.Position);
            ThrowItem(ev, pickup);
            Timing.RunCoroutine(RunCheck(ev.Player, pickup));
        }

        private IEnumerator<float> RunCheck(Player player, Pickup pickup)
        {
            for (int i = 0; i < 5; i++)
            {
                if (Round.IsEnded)
                    yield break;

                yield return Timing.WaitForSeconds(1f);
                Room room = Map.FindParentRoom(pickup.GameObject);
                if (room is null || room.Type != RoomType.HczArmory || pickup.Position.y > -1002f)
                    continue;

                pickup.Destroy();
                if (pickup.Type == ItemType.MicroHID)
                {
                    plugin.Config.CustomItems.SecondWind.Give(player);
                    yield break;
                }

                if (plugin.Config.CustomItems.WeaponToken.Check(pickup))
                {
                    plugin.Config.CustomItems.WeaponToken.GiveRandom(player);
                    yield break;
                }
            }
        }

        private void ThrowItem(DroppingItemEventArgs ev, Pickup pickup)
        {
            if (pickup.Base.Rb is not null && ev.IsThrown)
            {
                Vector3 vector = (ev.Player.ReferenceHub.playerMovementSync.PlayerVelocity / 3f) + (ev.Player.ReferenceHub.PlayerCameraReference.forward * 6f * (Mathf.Clamp01(Mathf.InverseLerp(7f, 0.1f, pickup.Base.Rb.mass)) + 0.3f));
                vector.x = Mathf.Max(Mathf.Abs(ev.Player.ReferenceHub.playerMovementSync.PlayerVelocity.x), Mathf.Abs(vector.x)) * ((vector.x < 0f) ? -1 : 1);
                vector.y = Mathf.Max(Mathf.Abs(ev.Player.ReferenceHub.playerMovementSync.PlayerVelocity.y), Mathf.Abs(vector.y)) * ((vector.y < 0f) ? -1 : 1);
                vector.z = Mathf.Max(Mathf.Abs(ev.Player.ReferenceHub.playerMovementSync.PlayerVelocity.z), Mathf.Abs(vector.z)) * ((vector.z < 0f) ? -1 : 1);
                pickup.Base.Rb.position = ev.Player.ReferenceHub.PlayerCameraReference.position;
                pickup.Base.Rb.velocity = vector;
                pickup.Base.Rb.angularVelocity = Vector3.Lerp(ev.Item.Base.ThrowSettings.RandomTorqueA, ev.Item.Base.ThrowSettings.RandomTorqueB, Random.value);
                float magnitude = pickup.Base.Rb.angularVelocity.magnitude;
                if (magnitude > pickup.Base.Rb.maxAngularVelocity)
                {
                    pickup.Base.Rb.maxAngularVelocity = magnitude;
                }
            }
        }
    }
}