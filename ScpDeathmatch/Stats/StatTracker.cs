// -----------------------------------------------------------------------
// <copyright file="StatTracker.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Stats
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.Stats.Components;
    using ScpDeathmatch.Stats.Models;
    using UnityEngine;

    /// <summary>
    /// Tracks the stats of players.
    /// </summary>
    public class StatTracker
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatTracker"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public StatTracker(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.Destroying += OnDestroying;
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Destroying -= OnDestroying;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }

        private void AddRoundKills(Player player)
        {
            if (!plugin.StatDatabase.TryGet(player, out PlayerInfo playerInfo))
                return;

            playerInfo.Kills += playerInfo.RoundKills;
            playerInfo.RoundKills = 0;
            plugin.StatDatabase.Upsert(playerInfo);
        }

        private void OnDestroying(DestroyingEventArgs ev)
        {
            AddRoundKills(ev.Player);
            if (ev.Player.GameObject.TryGetComponent(out StatComponent statComponent))
                Object.Destroy(statComponent);
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (!Round.IsStarted)
                return;

            if (plugin.StatDatabase.TryGet(ev.Target, out PlayerInfo targetInfo))
            {
                targetInfo.Deaths++;
                plugin.StatDatabase.Upsert(targetInfo);
            }

            if (ev.Killer is null || ev.Killer == ev.Target)
                return;

            if (plugin.StatDatabase.TryGet(ev.Killer, out PlayerInfo killerInfo))
            {
                killerInfo.RoundKills++;
                plugin.StatDatabase.Upsert(killerInfo);
            }
        }

        private void OnVerified(VerifiedEventArgs ev)
        {
            if (!plugin.StatDatabase.TryGet(ev.Player, out _))
                plugin.StatDatabase.Upsert(new PlayerInfo(ev.Player.UserId));

            ev.Player.GameObject.AddComponent<StatComponent>();
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (Player player in Player.List)
                AddRoundKills(player);
        }
    }
}