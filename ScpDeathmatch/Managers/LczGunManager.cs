// -----------------------------------------------------------------------
// <copyright file="LczGunManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using MEC;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Manages player's held items to respect the <see cref="Config.PreventLczGuns"/> config.
    /// </summary>
    public class LczGunManager : Subscribable
    {
        private CoroutineHandle coroutine;

        /// <summary>
        /// Initializes a new instance of the <see cref="LczGunManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public LczGunManager(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.ChangingItem -= OnChangingItem;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        private void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (Plugin.Config.PreventLczGuns && ev.NewItem is Firearm or MicroHid && ev.Player.Zone == ZoneType.LightContainment)
                ev.IsAllowed = false;
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (coroutine.IsRunning)
                Timing.KillCoroutines(coroutine);
        }

        private void OnRoundStarted()
        {
            if (coroutine.IsRunning)
                Timing.KillCoroutines(coroutine);

            coroutine = Timing.RunCoroutine(RunPrevention());
        }

        private IEnumerator<float> RunPrevention()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1f);
                if (!Plugin.Config.PreventLczGuns)
                    continue;

                foreach (Player player in Player.List)
                {
                    if (player.CurrentItem is Firearm or MicroHid && player.Zone == ZoneType.LightContainment)
                        player.CurrentItem = null;
                }
            }
        }
    }
}