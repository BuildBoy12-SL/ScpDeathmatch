// -----------------------------------------------------------------------
// <copyright file="GetNextStructure.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1313
    using System.Collections.Generic;
    using Exiled.API.Features;
    using HarmonyLib;
    using MapGeneration.Distributors;
    using NorthwoodLib.Pools;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="StructureDistributor.TryGetNextStructure"/> to respect <see cref="Config.BlacklistedGeneratorSpawns"/>.
    /// </summary>
    [HarmonyPatch(typeof(StructureDistributor), nameof(StructureDistributor.TryGetNextStructure))]
    internal static class GetNextStructure
    {
        private static bool Prefix(StructureDistributor __instance, out int structureId, out StructureSpawnpoint spawnpoint, ref bool __result)
        {
            spawnpoint = null;
            if (!__instance._queuedStructures.TryDequeue(out structureId))
            {
                __result = false;
                return false;
            }

            if (__instance._missingSpawnpoints.Contains(structureId))
            {
                __result = true;
                return false;
            }

            List<StructureSpawnpoint> structureSpawnpointList = ListPool<StructureSpawnpoint>.Shared.Rent();
            foreach (StructureSpawnpoint availableInstance in StructureSpawnpoint.AvailableInstances)
            {
                StructureType structureType = __instance.Settings.SpawnableStructures[structureId].StructureType;
                if (availableInstance == null || !availableInstance.CompatibleStructures.Contains(structureType))
                    continue;

                if (structureType == StructureType.Scp079Generator)
                {
                    Room room = Map.FindParentRoom(availableInstance.gameObject);
                    if (room != null && Plugin.Instance.Config.BlacklistedGeneratorSpawns.Contains(room.Type))
                        continue;
                }

                structureSpawnpointList.Add(availableInstance);
            }

            if (structureSpawnpointList.Count > 0)
            {
                StructureSpawnpoint structureSpawnpoint = structureSpawnpointList[Random.Range(0, structureSpawnpointList.Count)];
                StructureSpawnpoint.AvailableInstances.Remove(structureSpawnpoint);
                spawnpoint = structureSpawnpoint;
            }
            else
            {
                __instance._missingSpawnpoints.Add(structureId);
            }

            __result = true;
            return false;
        }
    }
}