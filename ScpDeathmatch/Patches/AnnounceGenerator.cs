// -----------------------------------------------------------------------
// <copyright file="AnnounceGenerator.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using HarmonyLib;
    using NorthwoodLib.Pools;

    /// <summary>
    /// Patches <see cref="Recontainer079.UpdateStatus"/> to override generator engaged announcements.
    /// </summary>
    [HarmonyPatch(typeof(Recontainer079), nameof(Recontainer079.UpdateStatus))]
    internal static class AnnounceGenerator
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Ldarg_0);
            newInstructions.RemoveRange(index, 4);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}