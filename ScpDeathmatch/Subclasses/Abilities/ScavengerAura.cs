// -----------------------------------------------------------------------
// <copyright file="ScavengerAura.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using InventorySystem.Items;
    using InventorySystem.Items.Pickups;
    using MEC;
    using ScpDeathmatch.Models;
    using UnityEngine;

    /// <inheritdoc />
    public class ScavengerAura : PassiveAbility
    {
        private static readonly int PickupMask = LayerMask.GetMask("Pickup");
        private readonly Dictionary<int, CoroutineHandle> coroutines = new();
        private readonly List<ushort> onCooldown = new();

        /// <summary>
        /// Gets or sets a value indicating whether the ability is currently enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public override string Name { get; set; } = "Scavenger Aura";

        /// <inheritdoc />
        public override string Description { get; set; } = "Automatically picks up ammo and items near you";

        /// <summary>
        /// Gets or sets the radius of the aura.
        /// </summary>
        public float Radius { get; set; } = 5f;

        /// <summary>
        /// Gets or sets the time, in seconds, between each check.
        /// </summary>
        public float RefreshRate { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the amount of time, in seconds, to wait before an item can be automatically picked up after being dropped.
        /// </summary>
        [Description("The amount of time, in seconds, to wait before an item can be automatically picked up after being dropped.")]
        public float DropCooldown { get; set; } = 2f;

        /// <summary>
        /// Gets or sets a value indicating whether keycards that have all of the same permissions as the player's current keycards should be picked up.
        /// </summary>
        [Description("Whether keycards that have all of the same permissions as the player's current keycards should be picked up")]
        public bool AllowDuplicateKeycards { get; set; } = false;

        /// <summary>
        /// Gets or sets the individual limits of items.
        /// </summary>
        [Description("The individual limits of items. If an item is not in the collection, it will not be picked up.")]
        public Dictionary<ItemType, Limit> ItemLimits { get; set; } = new()
        {
            { ItemType.KeycardScientist, new Limit(0, 2) },
        };

        /// <inheritdoc />
        protected override void AbilityAdded(Player player)
        {
            if (!IsEnabled)
                return;

            coroutines.Add(player.Id, Timing.RunCoroutine(RunAbility(player)));
            base.AbilityAdded(player);
        }

        /// <inheritdoc />
        protected override void AbilityRemoved(Player player)
        {
            if (coroutines.TryGetValue(player.Id, out CoroutineHandle coroutine))
            {
                Timing.KillCoroutines(coroutine);
                coroutines.Remove(player.Id);
            }

            base.AbilityRemoved(player);
        }

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            base.UnsubscribeEvents();
        }

        private static bool HasUniquePermissions(Player player, Item item)
        {
            if (item is not Keycard keycard)
                return true;

            KeycardPermissions combinedPermissions = 0;
            foreach (Item heldItem in player.Items)
            {
                if (heldItem is Keycard heldKeycard)
                    combinedPermissions |= heldKeycard.Permissions;
            }

            return (combinedPermissions & keycard.Permissions) != keycard.Permissions;
        }

        private static int GetItemCount(Player player, ItemType itemType)
        {
            if (itemType.IsAmmo())
            {
                player.Ammo.TryGetValue(itemType, out ushort ammoCount);
                return ammoCount;
            }

            return player.Items.Count(item => item.Type == itemType);
        }

        private void OnDroppingItem(DroppingItemEventArgs ev)
        {
            onCooldown.Add(ev.Item.Serial);
            Timing.CallDelayed(DropCooldown, () => onCooldown.Remove(ev.Item.Serial));
        }

        private void OnWaitingForPlayers()
        {
            foreach (CoroutineHandle coroutine in coroutines.Values)
                Timing.KillCoroutines(coroutine);

            coroutines.Clear();
            onCooldown.Clear();
        }

        private bool IsValidPickup(Player player, ItemPickupBase pickupBase, out Item item)
        {
            ItemBase itemBase = player.Inventory.CreateItemInstance(pickupBase.Info.ItemId, false);
            item = Item.Get(itemBase);
            if (!AllowDuplicateKeycards && !HasUniquePermissions(player, item))
                return false;

            if (!ItemLimits.TryGetValue(item.Type, out Limit limit) ||
                !limit.WithinLimit(GetItemCount(player, item.Type)))
                return false;

            return true;
        }

        private IEnumerator<float> RunAbility(Player player)
        {
            Collider[] colliders = new Collider[50];
            while (!Round.IsEnded)
            {
                yield return Timing.WaitForSeconds(RefreshRate);
                if (player.IsDead || player.SessionVariables.ContainsKey("PauseScavengerAura"))
                    continue;

                int colliderCount = Physics.OverlapSphereNonAlloc(player.Position, Radius, colliders, PickupMask);
                for (int i = 0; i < colliderCount; i++)
                {
                    ItemPickupBase pickupBase = colliders[i].GetComponentInParent<ItemPickupBase>();

                    if (pickupBase is null || onCooldown.Contains(pickupBase.Info.Serial))
                        continue;

                    if (!IsValidPickup(player, pickupBase, out Item item))
                        continue;

                    player.AddItem(item);
                    pickupBase.DestroySelf();

                    onCooldown.Add(pickupBase.Info.Serial);
                    Timing.CallDelayed(DropCooldown, () => onCooldown.Remove(pickupBase.Info.Serial));
                }
            }
        }
    }
}