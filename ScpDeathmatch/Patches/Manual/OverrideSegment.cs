// -----------------------------------------------------------------------
// <copyright file="OverrideSegment.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches.Manual
{
    using System.Reflection;
    using HarmonyLib;
    using MEC;

    /// <summary>
    /// Contains the harmony patch to override the default segment to <see cref="Segment.FixedUpdate"/> for all coroutines.
    /// </summary>
    internal class OverrideSegment : IManualPatch
    {
        /// <inheritdoc/>
        public void Patch(Harmony harmony)
        {
            MethodInfo runCoroutineInternal = typeof(Timing).GetMethod("RunCoroutineInternal", BindingFlags.NonPublic | BindingFlags.Instance);
            if (runCoroutineInternal is not null)
                harmony.Patch(runCoroutineInternal, new HarmonyMethod(typeof(OverrideSegment).GetMethod(nameof(Prefix), BindingFlags.NonPublic | BindingFlags.Static)));
        }

        private static void Prefix(ref Segment segment) => segment = Segment.FixedUpdate;
    }
}