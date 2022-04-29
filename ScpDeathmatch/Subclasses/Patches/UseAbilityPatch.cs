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
            Player player = Player.Get(sender);
            int abilityNumber = 0;
            if (arguments.Count > 0)
                int.TryParse(arguments.At(0), out abilityNumber);

            ReadOnlyCollection<CustomRole> roles = player.GetCustomRoles();
            ReadOnlyCollection<Subclass> subclasses = Subclass.GetSubclasses(player);
            if (roles.Count == 0 && subclasses.Count == 0)
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
                if (customRole.CustomAbilities.Count < abilityNumber + 1 || customRole.CustomAbilities[abilityNumber] is not ActiveAbility activeAbility || !activeAbility.CanUseAbility(player, out _))
                    continue;

                activeAbility.UseAbility(player);
                response = $"Ability {activeAbility.Name} used.";
                __result = true;
                return false;
            }

            foreach (Subclass subclass in subclasses)
            {
                if (subclass.CustomAbilities.Count < abilityNumber + 1 || subclass.CustomAbilities[abilityNumber] is not ActiveAbility activeAbility || !activeAbility.CanUseAbility(player, out _))
                    continue;

                activeAbility.UseAbility(player);
                response = $"Ability {activeAbility.Name} used.";
                __result = true;
                return false;
            }

            response = "Could not find an ability that was able to be used.";
            return false;
        }

        private static bool CheckCustomRoles(ArraySegment<string> arguments, Player sender, int abilityNumber, out string response)
        {
            CustomRole role = CustomRole.Get(arguments.At(1));
            if (role is null)
                return CheckSubclasses(arguments, sender, abilityNumber, out response);

            if (role.CustomAbilities.Count >= abilityNumber + 1)
            {
                if (role.CustomAbilities[abilityNumber] is ActiveAbility active)
                {
                    if (!active.CanUseAbility(sender, out response))
                        return false;

                    active.UseAbility(sender);
                    response = $"Ability {active.Name} used.";
                    return true;
                }
            }

            response = "Could not find an ability that was able to be used.";
            return false;
        }

        private static bool CheckSubclasses(ArraySegment<string> arguments, Player sender, int abilityNumber, out string response)
        {
            Subclass subclass = Subclass.Get(arguments.At(1));
            if (subclass is null)
            {
                response = $"The specified role {arguments.At(1)} does not exist.";
                return false;
            }

            if (subclass.CustomAbilities.Count >= abilityNumber + 1)
            {
                if (subclass.CustomAbilities[abilityNumber] is ActiveAbility active)
                {
                    if (!active.CanUseAbility(sender, out response))
                        return false;

                    active.UseAbility(sender);
                    response = $"Ability {active.Name} used.";
                    return true;
                }
            }

            response = "Could not find an ability that was able to be used.";
            return false;
        }
    }
}