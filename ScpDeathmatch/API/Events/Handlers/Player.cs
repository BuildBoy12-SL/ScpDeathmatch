// -----------------------------------------------------------------------
// <copyright file="Player.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.API.Events.Handlers
{
    using Exiled.Events.Extensions;
    using ScpDeathmatch.API.Events.EventArgs;
    using static Exiled.Events.Events;

    /// <summary>
    /// Player related events.
    /// </summary>
    public static class Player
    {
        /// <summary>
        /// Invoked after a <see cref="Exiled.API.Features.Player"/> has their max health changed.
        /// </summary>
        public static event CustomEventHandler<ChangingMaxHealthEventArgs> ChangingMaxHealth;

        /// <summary>
        /// Called after a <see cref="Exiled.API.Features.Player"/> has their max health changed.
        /// </summary>
        /// <param name="ev">The <see cref="ChangingMaxHealthEventArgs"/> instance.</param>
        public static void OnChangingMaxHealth(ChangingMaxHealthEventArgs ev) => ChangingMaxHealth.InvokeSafely(ev);
    }
}