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

    /// <summary>
    /// Manages the selection of subclasses at round start.
    /// </summary>
    public class SubclassSelectionManager
    {
        private readonly Plugin plugin;
        private readonly Dictionary<Player, ItemType> selectedItem = new();
        private readonly List<int> itemsInProgress = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SubclassSelectionManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public SubclassSelectionManager(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Player.TogglingFlashlight += OnTogglingFlashlight;
            Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.ChangingItem -= OnChangingItem;
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
            if (plugin.Config.ClassSelection.Selections is null || plugin.Config.ClassSelection.Selections.Count == 0)
                return;

            if (!Round.IsLobby || ev.NewItem is null ||
                !plugin.Config.ClassSelection.Selections.TryGetValue(ev.NewItem.Type, out SubclassSelection selection))
                return;

            selectedItem[ev.Player] = ev.NewItem.Type;
            if (string.IsNullOrEmpty(selection.Message))
                return;

            if (selection.IsBroadcast)
                ev.Player.Broadcast(3, selection.Message, shouldClearPrevious: true);
            else
                ev.Player.ShowHint(selection.Message);
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
            if (plugin.Config.ClassSelection.Selections is null || plugin.Config.ClassSelection.Selections.Count == 0)
                return;

            if (Round.IsLobby && !itemsInProgress.Contains(ev.Player.Id))
            {
                itemsInProgress.Add(ev.Player.Id);
                ev.Player.ClearInventory();
                Timing.CallDelayed(0.5f, () =>
                {
                    foreach (ItemType newItem in plugin.Config.ClassSelection.Selections.Keys)
                        ev.Player.AddItem(newItem);

                    itemsInProgress.Remove(ev.Player.Id);
                });
            }
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
            if (plugin.Config.ClassSelection.Selections is null || plugin.Config.ClassSelection.Selections.Count == 0)
                return;

            if (Round.IsStarted)
                plugin.Config.ClassSelection.Selections.Values.Random().GetSelection()?.AddRole(ev.Player);
        }

        private void OnRoundStarted()
        {
            if (plugin.Config.ClassSelection.Selections is null || plugin.Config.ClassSelection.Selections.Count == 0)
                return;

            itemsInProgress.Clear();
            foreach (Player player in Player.List)
            {
                if (player.SessionVariables.ContainsKey("IsNPC"))
                    continue;

                if (selectedItem.TryGetValue(player, out ItemType item) &&
                    plugin.Config.ClassSelection.Selections.TryGetValue(item, out SubclassSelection selection))
                {
                    Subclass subclass = selection.GetSelection();
                    if (subclass is not null)
                    {
                        subclass.AddRole(player);
                        continue;
                    }
                }

                plugin.Config.ClassSelection.Selections.Values.Random().GetSelection()?.AddRole(player);
            }

            selectedItem.Clear();
        }
    }
}