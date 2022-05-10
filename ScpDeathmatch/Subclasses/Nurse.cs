// -----------------------------------------------------------------------
// <copyright file="Nurse.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features;
    using MEC;
    using ScpDeathmatch.Models;

    /// <inheritdoc />
    public class Nurse : Subclass
    {
        private readonly Dictionary<Player, CoroutineHandle> healthCoroutines = new();

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Nurse);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc/>
        public override ConfiguredBadge Badge { get; set; } = new(nameof(Nurse), "pink");

        /// <inheritdoc />
        public override List<string> Inventory { get; set; } = new()
        {
            $"{ItemType.Medkit}",
        };

        /// <summary>
        /// Gets or sets the amount of time, in seconds, between each regeneration tick.
        /// </summary>
        [Description("The amount of time, in seconds, between each regeneration tick.")]
        public float SecondsPerTick { get; set; } = 2f;

        /// <summary>
        /// Gets or sets the amount of maximum health to regenerate per tick.
        /// </summary>
        [Description("The amount of maximum health to regenerate per tick.")]
        public int MaxHealthRegen { get; set; } = 1;

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
            {
                Timing.KillCoroutines(coroutineHandle);
                healthCoroutines.Remove(player);
            }

            base.RoleRemoved(player);
        }

        private IEnumerator<float> RunRegeneration(Player player)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(SecondsPerTick);
                int newMaximum = player.MaxHealth + MaxHealthRegen;
                player.MaxHealth = newMaximum > MaxHealth ? MaxHealth : newMaximum;
            }
        }
    }
}