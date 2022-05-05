// -----------------------------------------------------------------------
// <copyright file="GetNextStructure.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using HarmonyLib;
    using MapGeneration;
    using MapGeneration.Distributors;
    using NorthwoodLib.Pools;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="StructureDistributor.TryGetNextStructure"/> to respect <see cref="Configs.MapGenerationConfig.SpawnBlacklist"/>.
    /// </summary>
    [HarmonyPatch(typeof(StructureDistributor), nameof(StructureDistributor.TryGetNextStructure))]
    internal static class GetNextStructure
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Br_S);
            Label continueLabel = (Label)newInstructions[index].operand;

            const int offset = 4;
            index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ldloc_2) + offset;

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldloc_2),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(StructureDistributor), nameof(StructureDistributor.Settings))),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(SpawnablesDistributorSettings), nameof(SpawnablesDistributorSettings.SpawnableStructures))),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldind_I4),
                new CodeInstruction(OpCodes.Ldelem_Ref),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(SpawnableStructure), nameof(SpawnableStructure.StructureType))),
                new CodeInstruction(OpCodes.Call, Method(typeof(GetNextStructure), nameof(IsValidSpawnpoint))),
                new CodeInstruction(OpCodes.Brfalse_S, continueLabel),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }

        private static bool IsValidSpawnpoint(StructureSpawnpoint spawnpoint, StructureType structureType)
        {
            if (!Plugin.Instance.Config.MapGeneration.SpawnBlacklist.TryGetValue(structureType, out List<RoomName> blacklistedRooms))
                return true;

            RoomIdentifier roomIdentifier = RoomIdUtils.RoomAtPosition(spawnpoint.transform.position);
            if (roomIdentifier is null)
                return true;

            return !blacklistedRooms.Contains(roomIdentifier.Name);
        }
    }
}