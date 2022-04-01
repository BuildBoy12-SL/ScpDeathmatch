// -----------------------------------------------------------------------
// <copyright file="MicroHidHealing.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using InventorySystem.Items.MicroHID;
    using MEC;

    /// <summary>
    /// Manages the <see cref="ItemType.MicroHID"/> healing.
    /// </summary>
    public class MicroHidHealing
    {
        private readonly Plugin plugin;
        private readonly Dictionary<Player, CoroutineHandle> healingCoroutines = new Dictionary<Player, CoroutineHandle>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroHidHealing"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public MicroHidHealing(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.ChangingMicroHIDState += OnChangingMicroHIDState;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.ChangingMicroHIDState -= OnChangingMicroHIDState;
        }

        private void OnChangingMicroHIDState(ChangingMicroHIDStateEventArgs ev)
        {
            if (!plugin.Config.HealingMicro.IsEnabled || !ev.IsAllowed)
                return;

            switch (ev.NewState)
            {
                case HidState.PoweringUp:
                    healingCoroutines.Add(ev.Player, Timing.RunCoroutine(RunHealing(ev.Player)));
                    break;
                case HidState.PoweringDown:
                    if (healingCoroutines.TryGetValue(ev.Player, out CoroutineHandle coroutineHandle) && coroutineHandle.IsRunning)
                        Timing.KillCoroutines(coroutineHandle);
                    break;
                case HidState.Primed:
                case HidState.Firing:
                    ev.IsAllowed = false;
                    break;
            }
        }

        private IEnumerator<float> RunHealing(Player player)
        {
            while (Round.IsStarted)
            {
                player.Heal(plugin.Config.HealingMicro.HealthPerTick);
                yield return Timing.WaitForSeconds(plugin.Config.HealingMicro.SecondsPerTick);
            }
        }
    }
}