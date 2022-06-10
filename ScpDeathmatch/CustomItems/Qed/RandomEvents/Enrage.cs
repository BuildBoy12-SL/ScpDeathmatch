// -----------------------------------------------------------------------
// <copyright file="Enrage.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Roles;
    using Exiled.Events.EventArgs;
    using MEC;
    using PlayableScps;
    using ScpDeathmatch.CustomItems.Qed.Enums;

    /// <inheritdoc />
    public class Enrage : IRandomEvent
    {
        /// <inheritdoc />
        public string Name { get; set; } = nameof(Enrage);

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public RangeType Range { get; set; } = RangeType.Neutral;

        /// <inheritdoc />
        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            Timing.RunCoroutine(RunEnrage(ev.Thrower));
        }

        private static IEnumerator<float> RunEnrage(Player player)
        {
            RoleType previousRole = player.Role.Type;
            int previousMaxHp = player.MaxHealth;
            float previousHealth = player.Health;

            player.SetRole(RoleType.Scp096, SpawnReason.ForceClass, true);
            yield return Timing.WaitForSeconds(0.3f);

            if (player.Role is not Scp096Role scp096Role)
                yield break;

            foreach (Player ply in Player.List)
            {
                if (ply.IsHuman)
                    scp096Role.Script.AddTarget(ply.GameObject);
            }

            scp096Role.Script.Windup();
            yield return Timing.WaitUntilTrue(() => scp096Role.State == Scp096PlayerState.Docile);

            player.SetRole(previousRole, SpawnReason.ForceClass, true);
            yield return Timing.WaitForSeconds(0.3f);
            player.Health = previousHealth;
            player.MaxHealth = previousMaxHp;
        }
    }
}