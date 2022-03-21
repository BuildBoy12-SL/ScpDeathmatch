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
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;

    /// <summary>
    /// Handles the announcement of players per zone.
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

            foreach (ZoneType zoneType in Enum.GetValues(typeof(ZoneType)))
            {
                if (plugin.Config.ZoneAnnouncer.DisabledZones.Contains(zoneType))
                    continue;

                string playerCount = Player.Get(player => player.IsAlive && player.Zone == zoneType).Count().ToString();
                switch (zoneType)
                {
                    case ZoneType.Entrance:
                        Cassie.Message(plugin.Config.ZoneAnnouncer.EntranceAnnouncement.Replace("$PLAYERS", playerCount), isNoisy: !plugin.Config.ZoneAnnouncer.SuppressCassieNoise);
                        break;
                    case ZoneType.HeavyContainment:
                        Cassie.Message(plugin.Config.ZoneAnnouncer.HeavyContainmentAnnouncement.Replace("$PLAYERS", playerCount), isNoisy: !plugin.Config.ZoneAnnouncer.SuppressCassieNoise);
                        break;
                    case ZoneType.LightContainment:
                        Cassie.Message(plugin.Config.ZoneAnnouncer.LightContainmentAnnouncement.Replace("$PLAYERS", playerCount), isNoisy: !plugin.Config.ZoneAnnouncer.SuppressCassieNoise);
                        break;
                    case ZoneType.Surface:
                        Cassie.Message(plugin.Config.ZoneAnnouncer.SurfaceAnnouncement.Replace("$PLAYERS", playerCount), isNoisy: !plugin.Config.ZoneAnnouncer.SuppressCassieNoise);
                        break;
                    case ZoneType.Unspecified:
                        Cassie.Message(plugin.Config.ZoneAnnouncer.UnspecifiedAnnouncement.Replace("$PLAYERS", playerCount), isNoisy: !plugin.Config.ZoneAnnouncer.SuppressCassieNoise);
                        break;
                }
            }
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