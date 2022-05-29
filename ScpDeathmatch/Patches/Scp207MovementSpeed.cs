// -----------------------------------------------------------------------
// <copyright file="Scp207MovementSpeed.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using CustomPlayerEffects;
    using HarmonyLib;
    using NorthwoodLib.Pools;

    /// <summary>
    /// Patches <see cref="Scp207.GetMovementSpeed"/> to remove the base speed multiplier.
    /// </summary>
    [HarmonyPatch(typeof(Scp207), nameof(Scp207.GetMovementSpeed))]
    internal static class Scp207MovementSpeed
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            newInstructions.RemoveRange(0, 2);
            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Mul);
            newInstructions.RemoveAt(index);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}