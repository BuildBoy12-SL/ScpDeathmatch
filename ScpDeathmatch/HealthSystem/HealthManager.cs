// -----------------------------------------------------------------------
// <copyright file="HealthManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem
{
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.HealthSystem.Components;
    using ScpDeathmatch.Models;
    using UnityEngine;

    /// <summary>
    /// Manages the health system.
    /// </summary>
    public class HealthManager : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public HealthManager(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc/>
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Player.Destroying += OnDestroying;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
        }

        /// <inheritdoc/>
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Destroying -= OnDestroying;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
        }

        private void OnDestroying(DestroyingEventArgs ev)
        {
            if (ev.Player.GameObject.TryGetComponent(out HealthComponent healthComponent))
                Object.Destroy(healthComponent);
        }

        private void OnVerified(VerifiedEventArgs ev)
        {
            ev.Player.GameObject.AddComponent<HealthComponent>();
        }
    }
}