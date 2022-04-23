// -----------------------------------------------------------------------
// <copyright file="HealthManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem
{
    using Exiled.Events.EventArgs;
    using UnityEngine;

    /// <summary>
    /// Manages the health system.
    /// </summary>
    public class HealthManager
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public HealthManager(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.Destroying += OnDestroying;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Destroying -= OnDestroying;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
        }

        private void OnDestroying(DestroyingEventArgs ev)
        {
            if (ev.Player.GameObject.TryGetComponent(out RegenComponent regenComponent))
                Object.Destroy(regenComponent);
        }

        private void OnVerified(VerifiedEventArgs ev)
        {
            ev.Player.GameObject.AddComponent<RegenComponent>();
        }
    }
}