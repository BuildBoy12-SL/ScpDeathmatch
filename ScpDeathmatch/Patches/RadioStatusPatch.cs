// -----------------------------------------------------------------------
// <copyright file="RadioStatusPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1313
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.CustomItems.API.Features;
    using HarmonyLib;
    using InventorySystem.Items.Radio;
    using ScpDeathmatch.CustomItems;

    /// <summary>
    /// Patches <see cref="RadioItem.SendStatusMessage"/> to implement <see cref="SecondWind.Detonate"/>.
    /// </summary>
    [HarmonyPatch(typeof(RadioItem), nameof(RadioItem.SendStatusMessage))]
    internal static class RadioStatusPatch
    {
        private static void Postfix(RadioItem __instance)
        {
            if (__instance._enabled &&
                CustomItem.TryGet(Item.Get(__instance), out CustomItem customItem) &&
                customItem is SecondWind secondWind)
            {
                secondWind.Detonate(Player.Get(__instance.Owner));
            }
        }
    }
}