// -----------------------------------------------------------------------
// <copyright file="VisionControllerPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Recon.Patches
{
#pragma warning disable SA1313
    using System.Linq;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using HarmonyLib;
    using ScpDeathmatch.Subclasses.Subclasses.Recon.Abilities;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="Scp939_VisionController.FixedUpdate"/> to limit the distance of non-scp player's vision.
    /// </summary>
    [HarmonyPatch(typeof(Scp939_VisionController), nameof(Scp939_VisionController.FixedUpdate))]
    internal static class VisionControllerPatch
    {
        private static ToggleGoggles toggleGoggles;

        private static bool Prefix(Scp939_VisionController __instance)
        {
            Player player = Player.Get(__instance.gameObject);
            if (!Plugin.Instance.Config.Subclasses.Recon.Check(player))
                return true;

            toggleGoggles ??= Plugin.Instance.Config.Subclasses.Recon.CustomAbilities.FirstOrDefault(ability => ability.GetType() == typeof(ToggleGoggles)) as ToggleGoggles;
            if (toggleGoggles is null)
                return true;

            foreach (Visuals939 enabledEffect in Visuals939.EnabledEffects)
            {
                if (enabledEffect is null)
                    continue;

                if (enabledEffect.Hub.playerEffectsController.AllEffects.TryGetValue(typeof(Invisible), out PlayerEffect playerEffect) && playerEffect.IsEnabled)
                    continue;

                if (enabledEffect.Hub.characterClassManager.CurClass == RoleType.Spectator ||
                    Vector3.Distance(__instance.transform.position, enabledEffect.transform.position) < toggleGoggles.MaximumDistance)
                    __instance.AddVision(enabledEffect);
            }

            return false;
        }
    }
}