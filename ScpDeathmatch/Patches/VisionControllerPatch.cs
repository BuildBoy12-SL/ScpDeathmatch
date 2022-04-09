// -----------------------------------------------------------------------
// <copyright file="VisionControllerPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1313
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using HarmonyLib;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="Scp939_VisionController.FixedUpdate"/> to limit the distance of non-scp player's vision.
    /// </summary>
    [HarmonyPatch(typeof(Scp939_VisionController), nameof(Scp939_VisionController.FixedUpdate))]
    internal static class VisionControllerPatch
    {
        private static bool Prefix(Scp939_VisionController __instance)
        {
            Player player = Player.Get(__instance._myVisuals939.Hub);
            if (player is null || !Plugin.Instance.Config.CustomRoles.Recon.Check(player))
                return true;

            foreach (Visuals939 enabledEffect in Visuals939.EnabledEffects)
            {
                if (enabledEffect is not null && (enabledEffect.Hub.characterClassManager.CurClass == RoleType.Spectator || Vector3.Distance(__instance.transform.position, enabledEffect.transform.position) < Plugin.Instance.Config.CustomItems.ReconSwitch.MaximumDistance))
                    __instance.AddVision(enabledEffect);
            }

            __instance.noise = __instance.minimumNoiseLevel;
            __instance.UpdateVisions();
            return false;
        }
    }
}