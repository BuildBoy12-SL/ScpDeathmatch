// -----------------------------------------------------------------------
// <copyright file="MicroHidEffects.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.MicroHidEnhancers
{
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using InventorySystem.Items.MicroHID;
    using MEC;
    using ScpDeathmatch.Models;

    /// <inheritdoc />
    public class MicroHidEffects : Subscribable
    {
        private readonly Dictionary<Player, CoroutineHandle> effectCoroutines = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroHidEffects"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public MicroHidEffects(Plugin plugin)
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
            if (!Plugin.Config.MicroHid.MiscEffects.IsEnabled || !ev.IsAllowed)
                return;

            switch (ev.NewState)
            {
                case HidState.PoweringUp:
                    effectCoroutines[ev.Player] = Timing.RunCoroutine(RefreshEffects(ev.Player));
                    break;
                case HidState.PoweringDown:
                    if (effectCoroutines.TryGetValue(ev.Player, out CoroutineHandle coroutineHandle))
                        Timing.KillCoroutines(coroutineHandle);

                    break;
                case HidState.Primed:
                case HidState.Firing:
                    ev.IsAllowed = false;
                    break;
            }
        }

        private IEnumerator<float> RefreshEffects(Player player)
        {
            while (player.IsConnected)
            {
                yield return Timing.WaitForSeconds(1f);
                foreach (ConfiguredEffect effect in Plugin.Config.MicroHid.MiscEffects.Effects)
                {
                    PlayerEffect playerEffect = player.GetEffect(effect.Type);
                    playerEffect.Intensity = effect.Intensity;
                    playerEffect.Duration++;
                }
            }
        }
    }
}