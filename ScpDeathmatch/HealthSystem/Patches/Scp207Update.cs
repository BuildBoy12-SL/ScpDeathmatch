// -----------------------------------------------------------------------
// <copyright file="Scp207Update.cs" company="Build">
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
    /// Patches <see cref="Scp207.OnUpdate"/> to respect <see cref="HealthConfig.Scp207Damage"/>.
    /// </summary>
    [HarmonyPatch(typeof(Scp207), nameof(Scp207.OnUpdate))]
    internal static class Scp207Update
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label returnLabel = generator.DefineLabel();

            const int offset = 1;
            int index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Stfld) + offset;
            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                new(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.Health))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(HealthConfig), nameof(HealthConfig.Scp207Damage))),
                new(OpCodes.Brfalse_S, returnLabel),
            });

            newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}