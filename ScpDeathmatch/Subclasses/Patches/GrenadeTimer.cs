// -----------------------------------------------------------------------
// <copyright file="GrenadeTimer.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Patches
{
#pragma warning disable SA1313
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem.Items.ThrowableProjectiles;

    /// <summary>
    /// Patches <see cref="TimeGrenade.ServerActivate"/> to implement fuse times from the <see cref="Subclass"/> of the thrower.
    /// </summary>
    [HarmonyPatch(typeof(TimeGrenade), nameof(TimeGrenade.UserCode_RpcSetTime))]
    internal static class GrenadeTimer
    {
        private static void Prefix(TimeGrenade __instance, ref float time)
        {
            if (Subclass.Get(Player.Get(__instance.PreviousOwner.Hub)) is Subclass subclass &&
                subclass.TryGetFuseTime(__instance, out float fuseTime))
                time = fuseTime;
        }
    }
}