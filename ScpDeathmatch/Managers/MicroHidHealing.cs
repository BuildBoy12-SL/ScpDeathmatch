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
    using UnityEngine;

    /// <summary>
    /// Manages the <see cref="ItemType.MicroHID"/> healing.
    /// </summary>
    public class MicroHidHealing
    {
        private readonly Plugin plugin;
        private readonly Dictionary<Player, CoroutineHandle> healingCoroutines = new();

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
                    healingCoroutines[ev.Player] = Timing.RunCoroutine(RunHealing(ev.Player));
                    break;
                case HidState.PoweringDown:
                    if (healingCoroutines.TryGetValue(ev.Player, out CoroutineHandle coroutineHandle))
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
            yield return Timing.WaitForSeconds(plugin.Config.HealingMicro.InitialDelay);
            if (player.MaxArtificialHealth == 0f)
                player.AddAhp(0f, plugin.Config.HealingMicro.MaximumAhp, plugin.Config.HealingMicro.AhpDecayRate, plugin.Config.HealingMicro.AhpEfficacy, 0f, true);

            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(plugin.Config.HealingMicro.SecondsPerTick);
                float newHealth = player.Health + plugin.Config.HealingMicro.HealthPerTick;
                if (newHealth > player.MaxHealth)
                {
                    player.Health = player.MaxHealth;
                    player.ArtificialHealth = Mathf.Clamp((newHealth - player.MaxHealth) + player.ArtificialHealth, 0f, player.MaxArtificialHealth);
                    continue;
                }

                player.Health = newHealth;
            }
        }
    }
}