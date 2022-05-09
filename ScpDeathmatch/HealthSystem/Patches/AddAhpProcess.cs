// -----------------------------------------------------------------------
// <copyright file="AddAhpProcess.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem.Patches
{
#pragma warning disable SA1818
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using PlayerStatsSystem;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="AhpStat.ServerAddProcess(float,float,float,float,float,bool)"/> to take the highest ahp limit instead of the lowest.
    /// </summary>
    [HarmonyPatch(typeof(AhpStat), nameof(AhpStat.ServerAddProcess), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(bool))]
    internal static class AddAhpProcess
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindIndex(instruction => instruction.OperandIs(Method(typeof(Mathf), nameof(Mathf.Min), new[] { typeof(float), typeof(float) })));
            newInstructions[index] = new CodeInstruction(OpCodes.Call, Method(typeof(Mathf), nameof(Mathf.Max), new[] { typeof(float), typeof(float) }));

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}