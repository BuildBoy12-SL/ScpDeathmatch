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
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Stats.Components;
    using ScpDeathmatch.Stats.Models;
    using UnityEngine;

    /// <summary>
    /// Tracks the stats of players.
    /// </summary>
    public class StatTracker : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatTracker"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public StatTracker(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Player.Destroying += OnDestroying;
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Destroying -= OnDestroying;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }

        private void AddRoundKills(Player player)
        {
            if (!Plugin.StatDatabase.TryGet(player, out PlayerInfo playerInfo))
                return;

            playerInfo.Kills += playerInfo.RoundKills;
            playerInfo.RoundKills = 0;
            Plugin.StatDatabase.Upsert(playerInfo);
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

            if (!ev.Target.DoNotTrack && Plugin.StatDatabase.TryGet(ev.Target, out PlayerInfo targetInfo))
            {
                targetInfo.Deaths++;
                Plugin.StatDatabase.Upsert(targetInfo);
            }

            if (ev.Killer is null || ev.Killer == ev.Target)
                return;

            if (!ev.Killer.DoNotTrack && Plugin.StatDatabase.TryGet(ev.Killer, out PlayerInfo killerInfo))
            {
                killerInfo.RoundKills++;
                Plugin.StatDatabase.Upsert(killerInfo);
            }
        }

        private void OnVerified(VerifiedEventArgs ev)
        {
            if (ev.Player.DoNotTrack)
                return;

            if (!Plugin.StatDatabase.TryGet(ev.Player, out _))
                Plugin.StatDatabase.Upsert(new PlayerInfo(ev.Player.UserId));

            ev.Player.GameObject.AddComponent<StatComponent>();
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (Player player in Player.List)
            {
                if (player.SessionVariables.ContainsKey("IsNPC"))
                    continue;

                AddRoundKills(player);
            }
        }
    }
}