// -----------------------------------------------------------------------
// <copyright file="ScavengerAura.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.CustomRoles.API.Features;
    using InventorySystem.Items.Pickups;
    using MEC;
    using ScpDeathmatch.Models;
    using UnityEngine;

    /// <inheritdoc />
    public class ScavengerAura : PassiveAbility
    {
        private static readonly int PickupMask = LayerMask.GetMask("Pickup");
        private readonly Dictionary<Player, CoroutineHandle> coroutines = new Dictionary<Player, CoroutineHandle>();

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
        /// Gets or sets a value indicating whether keycards that have all of the same permissions as the player's current keycards should be picked up.
        /// </summary>
        [Description("Whether keycards that have all of the same permissions as the player's current keycards should be picked up")]
        public bool AllowDuplicateKeycards { get; set; } = false;

        /// <summary>
        /// Gets or sets the individual limits of items.
        /// </summary>
        [Description("The individual limits of items. If an item is not in the collection, it will not be picked up.")]
        public Dictionary<ItemType, Limit> ItemLimits { get; set; } = new Dictionary<ItemType, Limit>
        {
            { ItemType.KeycardScientist, new Limit(0, 2) },
        };

        /// <inheritdoc />
        protected override void AbilityAdded(Player player)
        {
            if (!IsEnabled)
                return;

            coroutines.Add(player, Timing.RunCoroutine(RunAbility(player)));
            base.AbilityAdded(player);
        }

        /// <inheritdoc />
        protected override void AbilityRemoved(Player player)
        {
            if (coroutines.TryGetValue(player, out CoroutineHandle coroutine))
            {
                Timing.KillCoroutines(coroutine);
                coroutines.Remove(player);
            }

            base.AbilityRemoved(player);
        }

        private IEnumerator<float> RunAbility(Player player)
        {
            Collider[] colliders = new Collider[50];
            while (IsEnabled && Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(RefreshRate);
                if (player.IsDead)
                    continue;

                int colliderCount = Physics.OverlapSphereNonAlloc(player.Position, Radius, colliders, PickupMask);
                for (int i = 0; i < colliderCount; i++)
                {
                    ItemPickupBase pickupBase = colliders[i].GetComponentInParent<ItemPickupBase>();
                    if (!IsValidPickup(player, pickupBase, out Pickup pickup))
                        continue;

                    player.AddItem(pickup);
                    pickup.Destroy();
                }
            }
        }

        private bool IsValidPickup(Player player, ItemPickupBase pickupBase, out Pickup pickup)
        {
            pickup = null;
            if (pickupBase is null)
                return false;

            pickup = Pickup.Get(pickupBase);
            if (!AllowDuplicateKeycards && !HasUniquePermissions(player, pickup.Type))
                return false;

            if (!ItemLimits.TryGetValue(pickup.Type, out Limit limit) ||
                !limit.WithinLimit(GetItemCount(player, pickup.Type)))
                return false;

            return true;
        }

        private bool HasUniquePermissions(Player player, ItemType itemType)
        {
            Item item = Item.Create(itemType);
            if (!(item is Keycard keycard))
                return true;

            KeycardPermissions combinedPermissions = 0;
            foreach (Item heldItem in player.Items)
            {
                if (heldItem is Keycard heldKeycard)
                    combinedPermissions |= heldKeycard.Permissions;
            }

            return (combinedPermissions & keycard.Permissions) != keycard.Permissions;
        }

        private int GetItemCount(Player player, ItemType itemType)
        {
            if (itemType.IsAmmo())
            {
                player.Ammo.TryGetValue(itemType, out ushort ammoCount);
                return ammoCount;
            }

            return player.Items.Count(item => item.Type == itemType);
        }
    }
}