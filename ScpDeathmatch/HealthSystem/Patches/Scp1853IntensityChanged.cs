// -----------------------------------------------------------------------
// <copyright file="Scp1853IntensityChanged.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using CustomPlayerEffects;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using ScpDeathmatch.Configs;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Scp1853.IntensityChanged"/> to respect <see cref="HealthConfig.Scp1853StaminaImmune"/>.
    /// </summary>
    [HarmonyPatch(typeof(Scp1853), nameof(Scp1853.IntensityChanged))]
    internal static class Scp1853IntensityChanged
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label skipStaminaLabel = generator.DefineLabel();

            const int offset = 1;
            int index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Blt_S) + offset;

            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                new(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.Health))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(HealthConfig), nameof(HealthConfig.Scp1853StaminaImmune))),
                new(OpCodes.Brtrue_S, skipStaminaLabel),
            });

            index = newInstructions.FindIndex(instruction => instruction.OperandIs(Field(typeof(Scp1853), nameof(Scp1853._staminaUsageMultiplier)))) + offset;
            newInstructions[index].labels.Add(skipStaminaLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}