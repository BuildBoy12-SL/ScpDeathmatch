// -----------------------------------------------------------------------
// <copyright file="AdrenalineActivated.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem.Patches
{
#pragma warning disable SA1313
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem.Items.Usables;

    /// <summary>
    /// Patches <see cref="Medkit.OnEffectsActivated"/> to remove the artificial health gain.
    /// </summary>
    [HarmonyPatch(typeof(Adrenaline), nameof(Adrenaline.OnEffectsActivated))]
    internal static class AdrenalineActivated
    {
        private static bool Prefix(Adrenaline __instance)
        {
            Plugin.Instance.Config.MedicalItems.Adrenaline.ApplyTo(Player.Get(__instance.Owner));
            __instance.Owner.playerEffectsController.UseMedicalItem(__instance.ItemTypeId);
            return false;
        }
    }
}