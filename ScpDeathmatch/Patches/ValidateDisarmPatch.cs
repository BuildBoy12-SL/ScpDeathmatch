// -----------------------------------------------------------------------
// <copyright file="ValidateDisarmPatch.cs" company="Build">
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
    using InventorySystem.Disarming;
    using NorthwoodLib.Pools;
    using ScpDeathmatch.Configs;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="DisarmedPlayers.ValidateEntry"/> to implement configs from <see cref="Config"/>.
    /// </summary>
    [HarmonyPatch(typeof(DisarmedPlayers), nameof(DisarmedPlayers.ValidateEntry))]
    internal static class ValidateDisarmPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label returnFalseLabel = generator.DefineLabel();
            Label returnTrueLabel = generator.DefineLabel();

            const int offset = 3;
            int index = newInstructions.FindIndex(i => i.opcode == OpCodes.Bne_Un_S) + offset;
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.Disarming))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(DisarmingConfig), nameof(DisarmingConfig.DisarmAtDistance))),
                new CodeInstruction(OpCodes.Brfalse_S, returnFalseLabel),
            });

            index = newInstructions.FindLastIndex(i => i.opcode == OpCodes.Ldloc_0);
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.Disarming))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(DisarmingConfig), nameof(DisarmingConfig.DropItems))),
                new CodeInstruction(OpCodes.Brfalse_S, returnTrueLabel),
            });

            index = newInstructions.FindLastIndex(i => i.opcode == OpCodes.Ldc_I4_0);
            newInstructions[index].labels.Add(returnFalseLabel);

            index = newInstructions.FindLastIndex(i => i.opcode == OpCodes.Ldc_I4_1);
            newInstructions[index].labels.Add(returnTrueLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}