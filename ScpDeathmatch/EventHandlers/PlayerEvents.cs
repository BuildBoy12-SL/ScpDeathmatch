﻿// -----------------------------------------------------------------------
// <copyright file="PlayerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.EventHandlers
{
    using CustomPlayerEffects;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using UnityEngine;
    using PlayerHandlers = Exiled.Events.Handlers.Player;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Player"/>.
    /// </summary>
    public class PlayerEvents
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEvents"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public PlayerEvents(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (ev.Target is null)
                return;

            if (!plugin.Config.DropEffectsOnDeath)
                return;

            byte scp207Intensity = ev.Target.GetEffectIntensity<Scp207>();
            byte scp1853Intensity = ev.Target.GetEffectIntensity<Scp1853>();
            Vector3 spawnPosition = ev.Target.Position + Vector3.up;

            for (int i = 0; i < scp207Intensity; i++)
                Item.Create(ItemType.SCP207).Spawn(spawnPosition);

            for (int i = 0; i < scp1853Intensity; i++)
                Item.Create(ItemType.SCP1853).Spawn(spawnPosition);
        }
    }
}