// -----------------------------------------------------------------------
// <copyright file="BodySlammingManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using PlayerStatsSystem;
    using ScpDeathmatch.API.Extensions;

    /// <summary>
    /// Handles body slamming from heights.
    /// </summary>
    public class BodySlammingManager
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="BodySlammingManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public BodySlammingManager(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Handler.Base is not UniversalDamageHandler universalDamageHandler ||
                universalDamageHandler.TranslationId != DeathTranslations.Falldown.Id)
                return;

            if (ev.Amount < plugin.Config.BodySlamming.MinimumDamage)
                return;

            Player target = Player.List.Closest(ev.Target.Position, plugin.Config.BodySlamming.MaximumDistance);
            if (target is null)
                return;

            ev.IsAllowed = false;
            target.Hurt(ev.Amount, $"Body Slammed by {ev.Target.DisplayNickname ?? ev.Target.Nickname}");
        }
    }
}