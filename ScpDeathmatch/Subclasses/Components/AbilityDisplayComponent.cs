// -----------------------------------------------------------------------
// <copyright file="AbilityDisplayComponent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Components
{
    using System.Collections.Generic;
    using System.Text;
    using AdvancedHints;
    using AdvancedHints.Enums;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using NorthwoodLib.Pools;
    using UnityEngine;

    /// <inheritdoc />
    public class AbilityDisplayComponent : MonoBehaviour
    {
        private Player player;
        private float globalTimer;

        private void Awake()
        {
            player = Player.Get(gameObject);
        }

        private void FixedUpdate()
        {
            globalTimer += Time.unscaledDeltaTime;
            if (globalTimer < 2f)
                return;

            globalTimer = 0f;
            if (player.IsDead || Subclass.Get(player) is not Subclass subclass)
                return;

            List<ActiveAbility> activeAbilities = ListPool<ActiveAbility>.Shared.Rent();
            foreach (CustomAbility ability in subclass.CustomAbilities)
            {
                if (ability is ActiveAbility activeAbility)
                    activeAbilities.Add(activeAbility);
            }

            if (activeAbilities.Count == 0)
            {
                ListPool<ActiveAbility>.Shared.Return(activeAbilities);
                return;
            }

            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            foreach (ActiveAbility ability in activeAbilities)
            {
                string canUse = ability.CanUseAbility(player, out _) ? "Ready" : "Recharging";
                stringBuilder.Append(ability.Name).Append(": ").AppendLine(canUse);
            }

            player.ShowManagedHint($"<align=right>{StringBuilderPool.Shared.ToStringReturn(stringBuilder)}</align>", 2.2f, true, DisplayLocation.Top);
            ListPool<ActiveAbility>.Shared.Return(activeAbilities);
        }
    }
}