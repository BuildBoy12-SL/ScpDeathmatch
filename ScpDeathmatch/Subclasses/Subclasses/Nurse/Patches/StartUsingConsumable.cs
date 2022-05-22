// -----------------------------------------------------------------------
// <copyright file="StartUsingConsumable.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Nurse.Patches
{
#pragma warning disable SA1313
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem.Items.Usables;

    /// <summary>
    /// Patches <see cref="Consumable.OnUsingStarted"/> to allow the <see cref="Nurse"/> subclass to instantly consume medical items.
    /// </summary>
    [HarmonyPatch(typeof(Consumable), nameof(Consumable.OnUsingStarted))]
    internal static class StartUsingConsumable
    {
        private static bool Prefix(Consumable __instance)
        {
            Player player = Player.Get(__instance.Owner);
            Subclass subclass = Subclass.Get(player);
            if (subclass is not Nurse)
                return true;

            __instance.ServerOnUsingCompleted();
            return false;
        }
    }
}