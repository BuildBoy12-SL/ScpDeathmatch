// -----------------------------------------------------------------------
// <copyright file="ServerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.EventHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Subclasses.Items;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Server"/>.
    /// </summary>
    public class ServerEvents : Subscribable
    {
        private CoroutineHandle coinCoroutine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerEvents"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public ServerEvents(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            ServerHandlers.RoundEnded += OnRoundEnded;
            ServerHandlers.RoundStarted += OnRoundStarted;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            ServerHandlers.RoundEnded -= OnRoundEnded;
            ServerHandlers.RoundStarted -= OnRoundStarted;
        }

        private void OnRoundStarted()
        {
            foreach (KeyValuePair<DoorType, float> kvp in Plugin.Config.DoorLocks)
                Door.Get(kvp.Key)?.Lock(kvp.Value, DoorLockType.AdminCommand);

            if (coinCoroutine.IsRunning)
                Timing.KillCoroutines(coinCoroutine);

            coinCoroutine = Timing.CallDelayed(Plugin.Config.CoinDeletionDelay, () =>
            {
                foreach (Pickup pickup in Map.Pickups)
                {
                    if (pickup.Type == ItemType.Coin && !CustomItem.TryGet(pickup, out _))
                        pickup.Destroy();
                }

                foreach (Player player in Player.List)
                {
                    foreach (Item item in player.Items.ToList())
                    {
                        if (item.Type == ItemType.Coin && (!CustomItem.TryGet(item, out CustomItem customItem) || customItem is ColaCoin))
                            player.RemoveItem(item);
                    }
                }
            });
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (coinCoroutine.IsRunning)
                Timing.KillCoroutines(coinCoroutine);
        }
    }
}