// -----------------------------------------------------------------------
// <copyright file="Scp207EffectsActivated.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1313
    using HarmonyLib;
    using InventorySystem.Items.Usables;
    using PlayerStatsSystem;

    /// <summary>
    /// Patches <see cref="Scp207.OnEffectsActivated"/> to adjust the maximum effect stack of Scp207.
    /// </summary>
    [HarmonyPatch(typeof(Scp207), nameof(Scp207.OnEffectsActivated))]
    internal static class Scp207EffectsActivated
    {
        private static bool Prefix(Scp207 __instance)
        {
            __instance.Owner.fpc.ModifyStamina(100f);
            __instance.Owner.playerStats.GetModule<HealthStat>().ServerHeal(30f);
            CustomPlayerEffects.Scp207 effect = __instance.Owner.playerEffectsController.GetEffect<CustomPlayerEffects.Scp207>();
            byte currentIntensity = effect != null ? effect.Intensity : byte.MaxValue;
            if (currentIntensity < effect.numberOfDrinks.Length - 1)
                __instance.Owner.playerEffectsController.ChangeEffectIntensity<CustomPlayerEffects.Scp207>(++currentIntensity);

            return false;
        }
    }
}