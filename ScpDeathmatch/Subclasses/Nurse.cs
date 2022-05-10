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
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using PlayerStatsSystem;
    using ScpDeathmatch.API.Events.EventArgs;
    using ScpDeathmatch.Models;
    using UnityEngine;

    /// <inheritdoc />
    public class Nurse : Subclass
    {
        private readonly Dictionary<Player, CoroutineHandle> healthCoroutines = new();
        private readonly Dictionary<Player, AhpStat.AhpProcess> ahpProcesses = new();

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

        /// <summary>
        /// Gets or sets the ahp settings. The limit is automatically adjusted to the player's lost max health.
        /// </summary>
        [Description("The ahp settings. The limit is automatically adjusted to the player's lost max health.")]
        public ConfiguredAhp Ahp { get; set; } = new(0f, 0f, -1f, 0.7f, 0f, true);

        /// <inheritdoc />
        protected override void RoleAdded(Player player)
        {
            healthCoroutines[player] = Timing.RunCoroutine(RunRegeneration(player));
            ahpProcesses[player] = Ahp.AddTo(player);
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

            ahpProcesses.Remove(player);
            base.RoleRemoved(player);
        }

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            API.Events.Handlers.Player.ChangingMaxHealth += OnChangingMaxHealth;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            API.Events.Handlers.Player.ChangingMaxHealth -= OnChangingMaxHealth;
            base.UnsubscribeEvents();
        }

        /// <inheritdoc />
        protected override void OnSpawned(SpawnedEventArgs ev)
        {
            if (Check(ev.Player) && !ev.Player.ActiveArtificialHealthProcesses.Any())
                ahpProcesses[ev.Player] = Ahp.AddTo(ev.Player);

            base.OnSpawned(ev);
        }

        private void OnChangingMaxHealth(ChangingMaxHealthEventArgs ev)
        {
            if (Check(ev.Player) && ahpProcesses.TryGetValue(ev.Player, out AhpStat.AhpProcess ahpProcess))
                ahpProcess.Limit = Mathf.Clamp(MaxHealth - ev.NewMaxHealth, 0, int.MaxValue);
        }

        private IEnumerator<float> RunRegeneration(Player player)
        {
            while (player.IsConnected)
            {
                yield return Timing.WaitForSeconds(SecondsPerTick);
                int newMaximum = player.MaxHealth + MaxHealthRegen;
                player.MaxHealth = newMaximum > MaxHealth ? MaxHealth : newMaximum;
            }
        }
    }
}