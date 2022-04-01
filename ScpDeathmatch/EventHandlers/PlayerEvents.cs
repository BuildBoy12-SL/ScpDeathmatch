// -----------------------------------------------------------------------
// <copyright file="PlayerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.EventHandlers
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

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
        /// Gets a collection pairing player objects to the amount of kills they have.
        /// </summary>
        public Dictionary<Player, int> Kills { get; } = new Dictionary<Player, int>();

        /// <summary>
        /// Gets or sets the player that killed first.
        /// </summary>
        public Player FirstBlood { get; set; }

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (ev.Killer == null || ev.Killer == ev.Target)
                return;

            if (FirstBlood == null)
                FirstBlood = ev.Killer;

            if (!Kills.ContainsKey(ev.Killer))
                Kills.Add(ev.Killer, 0);

            Kills[ev.Killer]++;
        }
    }
}