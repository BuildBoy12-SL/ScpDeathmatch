// -----------------------------------------------------------------------
// <copyright file="MedkitActivated.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem.Patches
{
#pragma warning disable SA1313
    using HarmonyLib;
    using InventorySystem.Items.Usables;
    using ScpDeathmatch.HealthSystem.Components;

    /// <summary>
    /// Patches <see cref="Medkit.OnEffectsActivated"/> to fully heal a player when they use a medkit.
    /// </summary>
    [HarmonyPatch(typeof(Medkit), nameof(Medkit.OnEffectsActivated))]
    internal static class MedkitActivated
    {
        private static bool Prefix(Medkit __instance)
        {
            if (__instance.Owner.gameObject.TryGetComponent(out HealthComponent healthComponent))
                healthComponent.Heal();

            __instance.Owner.playerEffectsController.UseMedicalItem(__instance.ItemTypeId);
            return false;
        }
    }
}