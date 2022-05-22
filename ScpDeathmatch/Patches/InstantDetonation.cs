// -----------------------------------------------------------------------
// <copyright file="InstantDetonation.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
    using Exiled.API.Features;
    using HarmonyLib;

    /// <summary>
    /// Patches <see cref="Warhead.Detonate"/> to make calls instantly detonate the warhead.
    /// </summary>
    [HarmonyPatch(typeof(Warhead), nameof(Warhead.Detonate))]
    internal static class InstantDetonation
    {
        private static void Postfix() => Warhead.Controller.NetworktimeToDetonation = 0.1f;
    }
}