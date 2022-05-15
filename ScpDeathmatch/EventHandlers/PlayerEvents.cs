// -----------------------------------------------------------------------
// <copyright file="PlayerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.EventHandlers
{
    using System.Linq;
    using CustomPlayerEffects;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using InventorySystem.Disarming;
    using MapGeneration.Distributors;
    using ScpDeathmatch.Models;
    using UnityEngine;
    using PlayerHandlers = Exiled.Events.Handlers.Player;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Player"/>.
    /// </summary>
    public class PlayerEvents : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEvents"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public PlayerEvents(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
            Exiled.Events.Handlers.Player.ProcessingHotkey += OnProcessingHotkey;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
            Exiled.Events.Handlers.Player.ProcessingHotkey -= OnProcessingHotkey;
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (!Plugin.Config.DropEffectsOnDeath || ev.Target is null)
                return;

            byte scp207Intensity = ev.Target.GetEffectIntensity<Scp207>();
            byte scp1853Intensity = ev.Target.GetEffectIntensity<Scp1853>();
            Vector3 spawnPosition = ev.Target.Position + Vector3.up;

            for (int i = 0; i < scp207Intensity; i++)
                Item.Create(ItemType.SCP207).Spawn(spawnPosition);

            for (int i = 0; i < scp1853Intensity; i++)
                Item.Create(ItemType.SCP1853).Spawn(spawnPosition);
        }

        private void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.Locker is PedestalScpLocker &&
                ev.Player.Items.Any(item => Plugin.Config.CanOpenPedestal.Contains(item.Type)))
                ev.IsAllowed = true;
        }

        private void OnProcessingHotkey(ProcessingHotkeyEventArgs ev)
        {
            if (DisarmedPlayers.Entries.Any(entry => entry.DisarmedPlayer == ev.Player.NetworkIdentity.netId))
                ev.IsAllowed = false;
        }
    }
}