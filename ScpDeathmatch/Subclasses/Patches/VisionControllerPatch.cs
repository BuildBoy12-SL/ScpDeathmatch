// -----------------------------------------------------------------------
// <copyright file="VisionControllerPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Patches
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
            Player player = Player.Get(__instance.gameObject);
            if (!Plugin.Instance.Config.Subclasses.Recon.Check(player))
                return true;

            float maximumDistance = Plugin.Instance.Config.ClientCommands.SubclassCommands.ToggleGoggles.MaximumDistance;
            foreach (Visuals939 enabledEffect in Visuals939.EnabledEffects)
            {
                if (enabledEffect is null)
                    continue;

                if (enabledEffect.Hub.playerEffectsController.AllEffects.TryGetValue(typeof(Invisible), out PlayerEffect playerEffect) && playerEffect.IsEnabled)
                    continue;

                if (enabledEffect.Hub.characterClassManager.CurClass == RoleType.Spectator ||
                    Vector3.Distance(__instance.transform.position, enabledEffect.transform.position) < maximumDistance)
                    __instance.AddVision(enabledEffect);
            }

            return false;
        }
    }
}