// -----------------------------------------------------------------------
// <copyright file="DisarmingLivesManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using InventorySystem.Disarming;
    using ScpDeathmatch.Models;
    using UnityEngine;

    /// <summary>
    /// Handles granting extra lives to players who have others disarmed.
    /// </summary>
    public class DisarmingLivesManager
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisarmingLivesManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public DisarmingLivesManager(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Handcuffing += OnHandcuffing;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Handcuffing -= OnHandcuffing;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (!plugin.Config.ExtraLives.IsEnabled || !ev.IsAllowed)
                return;

            if (!plugin.Config.ExtraLives.RespawnOnSuicide && (ev.Killer == null || ev.Killer == ev.Target))
                return;

            List<DisarmedPlayers.DisarmedEntry> disarmedEntries = DisarmedPlayers.Entries.Where(entry => entry.Disarmer == ev.Target.NetworkIdentity.netId).ToList();
            if (!disarmedEntries.Any())
                return;

            foreach (DisarmedPlayers.DisarmedEntry entry in disarmedEntries)
                Player.Get(entry.DisarmedPlayer)?.Kill(ev.Handler.Type);

            ev.IsAllowed = false;
            ev.Target.Health = ev.Target.MaxHealth;

            if (plugin.Config.ExtraLives.SpawnRagdolls)
            {
                RagdollInfo ragdollInfo = new RagdollInfo(Server.Host.ReferenceHub, ev.Handler.Base, ev.Target.Role.Type, ev.Target.Position, ev.Target.CameraTransform.rotation, ev.Target.DisplayNickname ?? ev.Target.Nickname, System.DateTime.Now.ToOADate());
                _ = new Ragdoll(ragdollInfo, true);
            }

            Vector3 newPosition = TeleportPosition.Get(ev.Target, plugin.Config.ExtraLives.TeleportType);
            if (newPosition != Vector3.zero)
                ev.Target.Position = newPosition + Vector3.up;

            ev.Target.Broadcast(plugin.Config.ExtraLives.ExtraLifeBroadcast);
        }

        private void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Cuffer == null)
                return;

            int disarmedEntries = DisarmedPlayers.Entries.Count(entry => entry.Disarmer == ev.Cuffer.NetworkIdentity.netId);
            ev.IsAllowed = disarmedEntries < plugin.Config.Disarming.DetainLimit;
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (plugin.Config.Disarming.DisarmedDamage || ev.Attacker == null || ev.Attacker == ev.Target)
                return;

            if (ev.Target.Inventory.IsDisarmed())
                ev.IsAllowed = false;
        }
    }
}