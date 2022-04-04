﻿// -----------------------------------------------------------------------
// <copyright file="CategoryLimitPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1313
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem.Configs;
    using ScpDeathmatch.CustomRoles;

    /// <summary>
    /// Patches <see cref="InventoryLimits.GetCategoryLimit(ItemCategory,ReferenceHub)"/> to implement <see cref="Scavenger.ItemLimits"/>.
    /// </summary>
    [HarmonyPatch(typeof(InventoryLimits), nameof(InventoryLimits.GetCategoryLimit), new[] { typeof(ItemCategory), typeof(ReferenceHub) })]
    internal static class CategoryLimitPatch
    {
        private static bool Prefix(ItemCategory category, ReferenceHub player, ref sbyte __result)
        {
            Scavenger scavenger = Plugin.Instance.Config.CustomRoles.Scavenger;
            if (!scavenger.Check(Player.Get(player)))
                return true;

            return !scavenger.ItemLimits.TryGetValue(category, out __result);
        }
    }
}