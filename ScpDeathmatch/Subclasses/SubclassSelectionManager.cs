// -----------------------------------------------------------------------
// <copyright file="SubclassSelectionManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;

    /// <summary>
    /// Manages the selection of subclasses at round start.
    /// </summary>
    public class SubclassSelectionManager
    {
        private readonly Plugin plugin;
        private readonly Dictionary<Player, ItemType> selectedItem = new();

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
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
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
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.TogglingFlashlight -= OnTogglingFlashlight;
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        private void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (Round.IsLobby && ev.NewItem is not null &&
                plugin.Config.ClassSelection.Selections.TryGetValue(ev.NewItem.Type, out SubclassSelection selection))
            {
                selectedItem[ev.Player] = ev.NewItem.Type;
                if (string.IsNullOrEmpty(selection.Message))
                    return;

                if (selection.IsBroadcast)
                    ev.Player.Broadcast(3, selection.Message, shouldClearPrevious: true);
                else
                    ev.Player.ShowHint(selection.Message);
            }
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!Round.IsLobby || ev.NewRole == RoleType.None)
                return;

            Timing.CallDelayed(0.5f, () =>
            {
                if (ev.Player.Role == ev.NewRole)
                    ev.Player.ResetInventory(plugin.Config.ClassSelection.Selections.Keys);
            });
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
            if (!Round.IsStarted)
                return;

            plugin.Config.ClassSelection.Selections.Values.ElementAt(UnityEngine.Random.Range(0, plugin.Config.ClassSelection.Selections.Values.Count)).GetSelection()?.AddRole(ev.Player);
        }

        private void OnRoundStarted()
        {
            if (plugin.Config.ClassSelection.Selections is null || plugin.Config.ClassSelection.Selections.Count == 0)
                return;

            foreach (Player player in Player.List)
            {
                if (selectedItem.TryGetValue(player, out ItemType item) &&
                    plugin.Config.ClassSelection.Selections.TryGetValue(item, out SubclassSelection selection))
                {
                    selection.GetSelection()?.AddRole(player);
                    continue;
                }

                plugin.Config.ClassSelection.Selections.Values.ElementAt(UnityEngine.Random.Range(0, plugin.Config.ClassSelection.Selections.Values.Count)).GetSelection()?.AddRole(player);
            }

            selectedItem.Clear();
        }
    }
}