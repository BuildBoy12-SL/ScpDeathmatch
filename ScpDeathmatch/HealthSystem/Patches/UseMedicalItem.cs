// -----------------------------------------------------------------------
// <copyright file="UseMedicalItem.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem.Patches
{
#pragma warning disable SA1313
    using CustomPlayerEffects;
    using HarmonyLib;
    using ScpDeathmatch.Configs;

    /// <summary>
    /// Patches <see cref="PlayerEffectsController.UseMedicalItem"/> to respect <see cref="MedicalItemsConfig.Scp500RemoveScp207"/> and <see cref="MedicalItemsConfig.Scp500RemoveScp1853"/>.
    /// </summary>
    [HarmonyPatch(typeof(PlayerEffectsController), nameof(PlayerEffectsController.UseMedicalItem))]
    internal static class UseMedicalItem
    {
        private static bool Prefix(PlayerEffectsController __instance, ItemType itemType)
        {
            foreach (PlayerEffect allEffect in __instance._allEffects)
            {
                if (allEffect is Scp207 && !Plugin.Instance.Config.MedicalItems.Scp500RemoveScp207)
                    continue;

                if (allEffect is Scp1853 && !Plugin.Instance.Config.MedicalItems.Scp500RemoveScp1853)
                    continue;

                if (allEffect is IHealablePlayerEffect healablePlayerEffect && healablePlayerEffect.IsHealable(itemType))
                    allEffect.Intensity = 0;
            }

            return false;
        }
    }
}