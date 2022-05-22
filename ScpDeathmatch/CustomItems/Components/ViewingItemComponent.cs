// -----------------------------------------------------------------------
// <copyright file="ViewingItemComponent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Components
{
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.CustomItems.API.Features;
    using InventorySystem.Items.Pickups;
    using ScpDeathmatch.Models;
    using UnityEngine;

    /// <inheritdoc />
    public class ViewingItemComponent : MonoBehaviour
    {
        private static readonly int PickupMask = LayerMask.GetMask("Pickup");
        private Player player;
        private float globalTimer;
        private ItemPickupBase currentlyLookingAt;

        private void Awake()
        {
            player = Player.Get(gameObject);
        }

        private void FixedUpdate()
        {
            globalTimer += Time.deltaTime;
            if (globalTimer > 1f)
            {
                CheckGround();
                globalTimer = 0f;
            }
        }

        private void CheckGround()
        {
            if (player is null || player.IsScp ||
                player.SessionVariables.ContainsKey("IsNPC") || player.IsDead)
                return;

            Hint hint = Plugin.Instance.Config.CustomItems.LookingAtMessage;
            if (!hint.Show)
                return;

            if (!Physics.Raycast(player.CameraTransform.position, player.CameraTransform.forward, out RaycastHit raycastHit, player.ReferenceHub.searchCoordinator.RayDistance, PickupMask))
            {
                currentlyLookingAt = null;
                return;
            }

            ItemPickupBase componentInParent = raycastHit.collider.gameObject.GetComponentInParent<ItemPickupBase>();
            if (currentlyLookingAt != componentInParent && CustomItem.TryGet(Pickup.Get(componentInParent), out CustomItem customItem))
            {
                currentlyLookingAt = componentInParent;
                AdvancedHints.Extensions.ShowManagedHint(player, string.Format(hint.Message, customItem.Name), hint.Duration, true, hint.DisplayLocation);
            }
        }
    }
}