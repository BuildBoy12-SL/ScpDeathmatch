// -----------------------------------------------------------------------
// <copyright file="Brute.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using ScpDeathmatch.Models;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class Brute : Subclass
    {
        private readonly Dictionary<Player, CoroutineHandle> healthCoroutines = new();

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc/>
        public override string Name { get; set; } = nameof(Brute);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override string Badge { get; set; } = nameof(Brute);

        /// <inheritdoc />
        public override string BadgeColor { get; set; } = "crimson";

        /// <summary>
        /// Gets or sets the amount of health per tick that should be healed.
        /// </summary>
        [Description("The amount of health per tick that should be healed.")]
        public float HealthPerTick { get; set; } = 0.5f;

        /// <summary>
        /// Gets or sets the amount of time, in seconds, that should pass to be considered a tick.
        /// </summary>
        [Description("The amount of time, in seconds, that should pass to be considered a tick.")]
        public float SecondsPerTick { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the ahp settings.
        /// </summary>
        public ConfiguredAhp Ahp { get; set; } = new(0f, 50f, -2f, 0.7f, 0f, true);

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; } = new();

        /// <inheritdoc />
        protected override void RoleAdded(Player player)
        {
            healthCoroutines[player] = Timing.RunCoroutine(RunRegeneration(player));
            base.RoleAdded(player);
        }

        /// <inheritdoc />
        protected override void RoleRemoved(Player player)
        {
            if (healthCoroutines.TryGetValue(player, out CoroutineHandle coroutineHandle))
                Timing.KillCoroutines(coroutineHandle);

            healthCoroutines.Remove(player);
            base.RoleRemoved(player);
        }

        /// <inheritdoc />
        protected override void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (Check(ev.Player))
                Timing.CallDelayed(1f, () => Ahp.AddTo(ev.Player));

            base.OnChangingRole(ev);
        }

        private IEnumerator<float> RunRegeneration(Player player)
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(SecondsPerTick);
                player.Heal(HealthPerTick);
            }
        }
    }
}