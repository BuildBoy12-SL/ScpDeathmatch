// -----------------------------------------------------------------------
// <copyright file="Player.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.API.Events.Handlers
{
    using Exiled.Events.Extensions;
    using InventorySystem.Items.Usables;
    using ScpDeathmatch.API.Events.EventArgs;
    using static Exiled.Events.Events;

    /// <summary>
    /// Player related events.
    /// </summary>
    public static class Player
    {
        /// <summary>
        /// Invoked before a <see cref="Player"/> activates a <see cref="Consumable"/>'s effects.
        /// </summary>
        public static event CustomEventHandler<ActivatingConsumableEffectsEventArgs> ActivatingConsumableEffects;

        /// <summary>
        /// Invoked after a <see cref="Exiled.API.Features.Player"/> has their max health changed.
        /// </summary>
        public static event CustomEventHandler<ChangingMaxHealthEventArgs> ChangingMaxHealth;

        /// <summary>
        /// Called before a <see cref="Player"/> activates a <see cref="Consumable"/>'s effects.
        /// </summary>
        /// <param name="ev">The <see cref="ActivatingConsumableEffectsEventArgs"/> instance.</param>
        public static void OnActivatingConsumableEffects(ActivatingConsumableEffectsEventArgs ev) => ActivatingConsumableEffects.InvokeSafely(ev);

        /// <summary>
        /// Called after a <see cref="Exiled.API.Features.Player"/> has their max health changed.
        /// </summary>
        /// <param name="ev">The <see cref="ChangingMaxHealthEventArgs"/> instance.</param>
        public static void OnChangingMaxHealth(ChangingMaxHealthEventArgs ev) => ChangingMaxHealth.InvokeSafely(ev);
    }
}