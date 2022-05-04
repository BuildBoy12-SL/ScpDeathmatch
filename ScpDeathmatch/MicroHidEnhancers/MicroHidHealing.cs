// -----------------------------------------------------------------------
// <copyright file="MicroHidHealing.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.MicroHidEnhancers
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using InventorySystem.Items.MicroHID;
    using MEC;
    using ScpDeathmatch.Models;
    using UnityEngine;

    /// <summary>
    /// Manages the <see cref="ItemType.MicroHID"/> healing.
    /// </summary>
    public class MicroHidHealing : Subscribable
    {
        private readonly Dictionary<Player, CoroutineHandle> healingCoroutines = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroHidHealing"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public MicroHidHealing(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Player.ChangingMicroHIDState += OnChangingMicroHIDState;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.ChangingMicroHIDState -= OnChangingMicroHIDState;
        }

        private void OnChangingMicroHIDState(ChangingMicroHIDStateEventArgs ev)
        {
            if (!Plugin.Config.HealingMicro.IsEnabled || !ev.IsAllowed)
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
            yield return Timing.WaitForSeconds(Plugin.Config.HealingMicro.InitialDelay);
            if (player.MaxArtificialHealth == 0f)
                player.AddAhp(0f, Plugin.Config.HealingMicro.MaximumAhp, Plugin.Config.HealingMicro.AhpDecayRate, Plugin.Config.HealingMicro.AhpEfficacy, 0f, true);

            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(Plugin.Config.HealingMicro.SecondsPerTick);
                float newHealth = player.Health + Plugin.Config.HealingMicro.HealthPerTick;
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