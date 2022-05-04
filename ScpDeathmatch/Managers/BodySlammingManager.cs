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
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles body slamming from heights.
    /// </summary>
    public class BodySlammingManager : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BodySlammingManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public BodySlammingManager(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Handler.Base is not UniversalDamageHandler universalDamageHandler ||
                universalDamageHandler.TranslationId != DeathTranslations.Falldown.Id)
                return;

            if (ev.Amount < Plugin.Config.BodySlamming.MinimumDamage)
                return;

            Player target = Player.List.Closest(ev.Target.Position, Plugin.Config.BodySlamming.MaximumDistance, player => player != ev.Target && !player.SessionVariables.ContainsKey("IsNPC"));
            if (target is null)
                return;

            ev.IsAllowed = false;
            target.Hurt(ev.Amount, $"Body Slammed by {ev.Target.DisplayNickname ?? ev.Target.Nickname}");
        }
    }
}