// -----------------------------------------------------------------------
// <copyright file="SubclassSelectionManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using ScpDeathmatch.API.Extensions;
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Subclasses.Components;
    using UnityEngine;

    /// <summary>
    /// Manages the selection of subclasses at round start.
    /// </summary>
    public class SubclassSelectionManager : Subscribable
    {
        private readonly Dictionary<Player, ItemType> selectedItem = new();
        private readonly List<int> itemsInProgress = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SubclassSelectionManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public SubclassSelectionManager(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;
            Exiled.Events.Handlers.Player.Destroying += OnDestroying;
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Player.TogglingFlashlight += OnTogglingFlashlight;
            Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.ChangingItem -= OnChangingItem;
            Exiled.Events.Handlers.Player.Destroying -= OnDestroying;
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            Exiled.Events.Handlers.Player.TogglingFlashlight -= OnTogglingFlashlight;
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        private void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (Plugin.Config.ClassSelection.Selections is null || Plugin.Config.ClassSelection.Selections.Count == 0)
                return;

            if (!Round.IsLobby || ev.NewItem is null ||
                !Plugin.Config.ClassSelection.Selections.TryGetValue(ev.NewItem.Type, out SubclassSelection selection))
                return;

            selectedItem[ev.Player] = ev.NewItem.Type;
            if (string.IsNullOrEmpty(selection.Message))
                return;

            if (selection.IsBroadcast)
                ev.Player.Broadcast(3, selection.Message, shouldClearPrevious: true);
            else
                ev.Player.ShowHint(selection.Message);
        }

        private void OnDestroying(DestroyingEventArgs ev)
        {
            if (ev.Player.GameObject.TryGetComponent(out AbilityDisplayComponent abilityDisplayComponent))
                Object.Destroy(abilityDisplayComponent);
        }

        private void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (Round.IsLobby)
                ev.IsAllowed = false;
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            if (Round.IsLobby)
                ev.IsAllowed = false;
        }

        private void OnSpawned(SpawnedEventArgs ev)
        {
            if (Plugin.Config.ClassSelection.Selections is null || Plugin.Config.ClassSelection.Selections.Count == 0)
                return;

            if (!Round.IsLobby || itemsInProgress.Contains(ev.Player.Id))
                return;

            itemsInProgress.Add(ev.Player.Id);
            ev.Player.ClearInventory();
            Timing.CallDelayed(0.5f, () =>
            {
                if (!ev.Player.IsConnected)
                    return;

                foreach (ItemType newItem in Plugin.Config.ClassSelection.Selections.Keys)
                    ev.Player.AddItem(newItem);

                itemsInProgress.Remove(ev.Player.Id);
            });
        }

        private void OnTogglingFlashlight(TogglingFlashlightEventArgs ev)
        {
            if (Round.IsLobby)
                ev.IsAllowed = false;
        }

        private void OnUsingItem(UsingItemEventArgs ev)
        {
            if (Round.IsLobby)
                ev.IsAllowed = false;
        }

        private void OnVerified(VerifiedEventArgs ev)
        {
            ev.Player.GameObject.AddComponent<AbilityDisplayComponent>();
            if (Plugin.Config.ClassSelection.Selections is null || Plugin.Config.ClassSelection.Selections.Count == 0 || !Round.IsStarted)
                return;

            Timing.CallDelayed(1f, () =>
            {
                Plugin.Config.Subclasses.Insurgent.AddRole(ev.Player);
                ev.Player.Role.Type = RoleType.Scp079;
            });
        }

        private void OnRoundStarted()
        {
            if (Plugin.Config.ClassSelection.Selections is null || Plugin.Config.ClassSelection.Selections.Count == 0)
                return;

            itemsInProgress.Clear();
            foreach (Player player in Player.List)
            {
                if (player.SessionVariables.ContainsKey("IsNPC"))
                    continue;

                if (selectedItem.TryGetValue(player, out ItemType item) &&
                    Plugin.Config.ClassSelection.Selections.TryGetValue(item, out SubclassSelection selection))
                {
                    Subclass subclass = selection.GetSelection();
                    if (subclass is not null)
                    {
                        subclass.AddRole(player);
                        continue;
                    }
                }

                Plugin.Config.ClassSelection.Selections.Values.Random().GetSelection()?.AddRole(player);
            }

            selectedItem.Clear();
        }
    }
}