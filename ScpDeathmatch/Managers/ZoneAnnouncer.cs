// -----------------------------------------------------------------------
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
                if (plugin.Config.ZoneAnnouncer.DisabledZones.Contains(zoneType))
                    continue;

                int playerCount = Player.Get(player => player.IsAlive && player.Zone == zoneType).Count();
                if (playerCount < 1)
                    continue;

                switch (zoneType)
                {
                    case ZoneType.Entrance:
                        announcementBuilder.Append(plugin.Config.ZoneAnnouncer.EntranceAnnouncement.Replace("$PLAYERS", playerCount.ToString()));
                        break;
                    case ZoneType.HeavyContainment:
                        announcementBuilder.Append(plugin.Config.ZoneAnnouncer.HeavyContainmentAnnouncement.Replace("$PLAYERS", playerCount.ToString()));
                        break;
                    case ZoneType.LightContainment:
                        announcementBuilder.Append(plugin.Config.ZoneAnnouncer.LightContainmentAnnouncement.Replace("$PLAYERS", playerCount.ToString()));
                        break;
                    case ZoneType.Surface:
                        announcementBuilder.Append(plugin.Config.ZoneAnnouncer.SurfaceAnnouncement.Replace("$PLAYERS", playerCount.ToString()));
                        break;
                    case ZoneType.Unspecified:
                        announcementBuilder.Append(plugin.Config.ZoneAnnouncer.UnspecifiedAnnouncement.Replace("$PLAYERS", playerCount.ToString()));
                        break;
                }

                announcementBuilder.Append(" . ");
            }

            Cassie.Message(StringBuilderPool.Shared.ToStringReturn(announcementBuilder), !plugin.Config.ZoneAnnouncer.SuppressCassieNoise);
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