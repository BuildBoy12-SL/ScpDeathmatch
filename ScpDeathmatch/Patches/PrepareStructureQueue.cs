// -----------------------------------------------------------------------
// <copyright file="PrepareStructureQueue.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1313
    using HarmonyLib;
    using MapGeneration.Distributors;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Patches <see cref="StructureDistributor.PrepareQueueForStructures"/> to manipulate the amount of structures to spawn.
    /// </summary>
    [HarmonyPatch(typeof(StructureDistributor), nameof(StructureDistributor.PrepareQueueForStructures))]
    internal static class PrepareStructureQueue
    {
        private static void Prefix(StructureDistributor __instance)
        {
            foreach (SpawnableStructure structure in __instance.Settings.SpawnableStructures)
            {
                if (Plugin.Instance.Config.MapGeneration.StructureLimits.TryGetValue(structure.StructureType, out Limit limit))
                {
                    structure.MinAmount = limit.Min;
                    structure.MaxAmount = limit.Max;
                }
            }
        }
    }
}