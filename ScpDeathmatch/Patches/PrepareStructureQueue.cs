// -----------------------------------------------------------------------
// <copyright file="PrepareStructureQueue.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1313
    using System.Collections.Generic;
    using HarmonyLib;
    using MapGeneration.Distributors;
    using NorthwoodLib.Pools;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="StructureDistributor.PrepareQueueForStructures"/> to manipulate the amount of structures to spawn.
    /// </summary>
    [HarmonyPatch(typeof(StructureDistributor), nameof(StructureDistributor.PrepareQueueForStructures))]
    internal static class PrepareStructureQueue
    {
        private static bool Prefix(StructureDistributor __instance)
        {
            List<int> list = ListPool<int>.Shared.Rent();
            for (int index1 = 0; index1 < __instance.Settings.SpawnableStructures.Length; ++index1)
            {
                SpawnableStructure structure = __instance.Settings.SpawnableStructures[index1];

                if (structure is Scp079Generator)
                {
                    structure.MinAmount = Plugin.Instance.Config.GeneratorsToSpawn;
                    structure.MaxAmount = Plugin.Instance.Config.GeneratorsToSpawn;
                }

                int num = Random.Range(structure.MinAmount, structure.MaxAmount + 1);
                for (int index2 = 0; index2 < num; ++index2)
                {
                    if (index2 < structure.MinAmount)
                        __instance._queuedStructures.Enqueue(index1);
                    else
                        list.Add(index1);
                }
            }

            while (list.Count > 0)
            {
                int index = Random.Range(0, list.Count);
                __instance._queuedStructures.Enqueue(list[index]);
                list.RemoveAt(index);
            }

            ListPool<int>.Shared.Return(list);
            return false;
        }
    }
}