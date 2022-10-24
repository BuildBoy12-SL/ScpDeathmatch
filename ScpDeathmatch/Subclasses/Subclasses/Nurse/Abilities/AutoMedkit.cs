// -----------------------------------------------------------------------
// <copyright file="AutoMedkit.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Nurse.Abilities
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using MEC;

    /// <summary>
    /// An ability that automatically resupplies the player with medkits.
    /// </summary>
    public class AutoMedkit : PassiveAbility
    {
        private readonly Dictionary<int, CoroutineHandle> coroutines = new();
        private readonly Dictionary<int, int> cooldowns = new();

        /// <summary>
        /// Gets or sets a value indicating whether the ability is currently enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public override string Name { get; set; } = "AutoMedkit";

        /// <inheritdoc />
        public override string Description { get; set; } = "Automatically grants a medkit if the user has not had one for 30 seconds";

        /// <summary>
        /// Gets or sets the amount of seconds to wait before granting a medkit.
        /// </summary>
        public int Threshold { get; set; } = 30;

        /// <inheritdoc />
        protected override void AbilityAdded(Player player)
        {
            if (IsEnabled)
                coroutines.Add(player.Id, Timing.RunCoroutine(RunAbility(player)));
        }

        /// <inheritdoc />
        protected override void AbilityRemoved(Player player)
        {
            if (!coroutines.TryGetValue(player.Id, out CoroutineHandle coroutine))
                return;

            Timing.KillCoroutines(coroutine);
            coroutines.Remove(player.Id);
        }

        private IEnumerator<float> RunAbility(Player player)
        {
            while (!Round.IsEnded)
            {
                yield return Timing.WaitForSeconds(1f);
                if (!cooldowns.ContainsKey(player.Id))
                    cooldowns.Add(player.Id, 0);

                if (++cooldowns[player.Id] >= Threshold && player.Items.Count < 8)
                {
                    cooldowns[player.Id] = 0;
                    player.AddItem(ItemType.Medkit);
                }
            }
        }
    }
}