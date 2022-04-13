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
    using Exiled.API.Features.Attributes;
    using Exiled.CustomRoles.API.Features;
    using MEC;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class Brute : Subclass
    {
        private readonly Dictionary<Player, CoroutineHandle> healthCoroutines = new();

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 135;

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