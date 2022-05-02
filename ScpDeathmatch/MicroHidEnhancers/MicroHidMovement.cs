// -----------------------------------------------------------------------
// <copyright file="MicroHidMovement.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.MicroHidEnhancers
{
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using InventorySystem.Items.MicroHID;
    using MEC;

    /// <summary>
    /// Manages the <see cref="ItemType.MicroHID"/> movement enhancement.
    /// </summary>
    public class MicroHidMovement
    {
        private readonly Plugin plugin;
        private readonly Dictionary<Player, CoroutineHandle> movementCoroutines = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroHidMovement"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public MicroHidMovement(Plugin plugin) => this.plugin = plugin;

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
            if (!plugin.Config.SpeedyMicro.IsEnabled || !ev.IsAllowed)
                return;

            CoroutineHandle coroutineHandle;
            switch (ev.NewState)
            {
                case HidState.PoweringUp:
                    if (movementCoroutines.TryGetValue(ev.Player, out coroutineHandle))
                        Timing.KillCoroutines(coroutineHandle);

                    movementCoroutines[ev.Player] = Timing.RunCoroutine(RunMovementIncrease(ev.Player));
                    break;
                case HidState.PoweringDown:
                    if (movementCoroutines.TryGetValue(ev.Player, out coroutineHandle))
                        Timing.KillCoroutines(coroutineHandle);

                    movementCoroutines[ev.Player] = Timing.RunCoroutine(RunMovementDecrease(ev.Player));
                    break;
                case HidState.Primed:
                case HidState.Firing:
                    ev.IsAllowed = false;
                    break;
            }
        }

        private IEnumerator<float> RunMovementIncrease(Player player)
        {
            yield return Timing.WaitForSeconds(plugin.Config.SpeedyMicro.InitialDelay);

            PlayerEffect movementBoost = player.GetEffect(EffectType.MovementBoost);
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(plugin.Config.SpeedyMicro.SecondsPerTick);
                byte newIntensity = (byte)(movementBoost.Intensity + plugin.Config.SpeedyMicro.BoostIncreasePerTick);
                if (newIntensity > plugin.Config.SpeedyMicro.MaximumBoost)
                {
                    movementBoost.Intensity = plugin.Config.SpeedyMicro.MaximumBoost;
                    continue;
                }

                movementBoost.Intensity = newIntensity;
            }
        }

        private IEnumerator<float> RunMovementDecrease(Player player)
        {
            PlayerEffect movementBoost = player.GetEffect(EffectType.MovementBoost);
            while (movementBoost.IsEnabled)
            {
                yield return Timing.WaitForSeconds(plugin.Config.SpeedyMicro.SecondsPerTick);
                movementBoost.Intensity -= plugin.Config.SpeedyMicro.BoostDecreasePerTick;
            }
        }
    }
}