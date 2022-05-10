// -----------------------------------------------------------------------
// <copyright file="UseAbilityPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Patches
{
#pragma warning disable SA1313
    using System;
    using System.Collections.ObjectModel;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API;
    using Exiled.CustomRoles.API.Features;
    using Exiled.CustomRoles.Commands;
    using HarmonyLib;

    /// <summary>
    /// Patches <see cref="UseAbility.Execute"/> to implement the usage of subclass abilities.
    /// </summary>
    [HarmonyPatch(typeof(UseAbility), nameof(UseAbility.Execute))]
    internal static class UseAbilityPatch
    {
        private static bool Prefix(ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
        {
            response = string.Empty;
            Player player = Player.Get(sender);
            int abilityNumber = 0;
            if (arguments.Count > 0)
                int.TryParse(arguments.At(0), out abilityNumber);

            ReadOnlyCollection<CustomRole> roles = player.GetCustomRoles();
            Subclass subclass = Subclass.Get(player);
            if (roles.Count == 0 && subclass is null)
            {
                response = "You do not have any custom roles.";
                __result = false;
                return false;
            }

            if (arguments.Count > 1)
            {
                __result = CheckCustomRoles(arguments, player, abilityNumber, out response);
                if (__result)
                    return false;
            }

            foreach (CustomRole customRole in roles)
            {
                if (customRole.CustomAbilities.Count < abilityNumber + 1 || customRole.CustomAbilities[abilityNumber] is not ActiveAbility customRoleAbility || !customRoleAbility.CanUseAbility(player, out _))
                    continue;

                customRoleAbility.UseAbility(player);
                response = $"Ability {customRoleAbility.Name} used.";
                __result = true;
                return false;
            }

            if (subclass.CustomAbilities.Count < abilityNumber + 1 || subclass.CustomAbilities[abilityNumber] is not ActiveAbility subclassAbility || !subclassAbility.CanUseAbility(player, out response))
                return false;

            subclassAbility.UseAbility(player);
            response = $"Ability {subclassAbility.Name} used.";
            __result = true;
            return false;
        }

        private static bool CheckCustomRoles(ArraySegment<string> arguments, Player sender, int abilityNumber, out string response)
        {
            CustomRole role = CustomRole.Get(arguments.At(1));
            if (role is null)
            {
                response = $"'{arguments.At(1)}' is not a custom role.";
                return false;
            }

            response = "Could not find an ability that was able to be used.";
            if (role.CustomAbilities.Count < abilityNumber + 1 ||
                role.CustomAbilities[abilityNumber] is not ActiveAbility active)
                return false;

            if (!active.CanUseAbility(sender, out response))
                return false;

            active.UseAbility(sender);
            response = $"Ability {active.Name} used.";
            return true;
        }
    }
}