﻿// -----------------------------------------------------------------------
// <copyright file="ZoneAnnouncer.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using NorthwoodLib.Pools;

    /// <summary>
    /// Handles the announcement of player counts.
    /// </summary>
    public class ZoneAnnouncer
    {
        private readonly Plugin plugin;
        private CoroutineHandle announcementCoroutine;
        private bool isFirstTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneAnnouncer"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public ZoneAnnouncer(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        /// <summary>
        /// Announces the amount of players in each zone.
        /// </summary>
        public void Announce()
        {
            if (!plugin.Config.ZoneAnnouncer.IsEnabled)
                return;

            StringBuilder announcementBuilder = StringBuilderPool.Shared.Rent();

            foreach (ZoneType zoneType in Enum.GetValues(typeof(ZoneType)))
            {
                if (!plugin.Config.ZoneAnnouncer.Announcements.TryGetValue(zoneType, out string announcement) ||
                    string.IsNullOrEmpty(announcement))
                    continue;

                int playerCount = Player.Get(player => player.IsAlive && player.Role.Type != RoleType.Scp079 && player.Zone == zoneType).Count();
                if (playerCount < 1)
                    continue;

                announcementBuilder
                    .Append(announcement.Replace("$PLAYERS", playerCount.ToString()))
                    .Append(" . ");
            }

            string builtAnnouncement = StringBuilderPool.Shared.ToStringReturn(announcementBuilder);
            if (string.IsNullOrEmpty(builtAnnouncement))
                return;

            if (!string.IsNullOrEmpty(plugin.Config.ZoneAnnouncer.StartupNoise))
                builtAnnouncement = plugin.Config.ZoneAnnouncer.StartupNoise + " " + builtAnnouncement;

            Cassie.Message(builtAnnouncement, isNoisy: !plugin.Config.ZoneAnnouncer.SuppressCassieNoise);
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (announcementCoroutine.IsRunning)
                Timing.KillCoroutines(announcementCoroutine);
        }

        private void OnRoundStarted()
        {
            if (announcementCoroutine.IsRunning)
                Timing.KillCoroutines(announcementCoroutine);

            announcementCoroutine = Timing.RunCoroutine(RunAnnouncer());
        }

        private IEnumerator<float> RunAnnouncer()
        {
            isFirstTime = true;
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(isFirstTime ? plugin.Config.ZoneAnnouncer.AnnouncerFirstDelay : plugin.Config.ZoneAnnouncer.AnnouncerDelay);
                Announce();
                isFirstTime = false;
            }
        }
    }
}