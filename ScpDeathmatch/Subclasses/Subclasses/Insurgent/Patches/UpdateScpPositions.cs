﻿// -----------------------------------------------------------------------
// <copyright file="UpdateScpPositions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Insurgent.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Exiled.API.Features;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using ScpDeathmatch.Configs;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Scp079PlayerScript.UpdateScpPositions"/> to update all player positions for the <see cref="Insurgent"/> subclass.
    /// </summary>
    [HarmonyPatch(typeof(Scp079PlayerScript), nameof(Scp079PlayerScript.UpdateScpPositions))]
    internal static class UpdateScpPositions
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label addPositionLabel = generator.DefineLabel();
            Label skipBypassLabel = generator.DefineLabel();

            int offset = 1;
            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Beq_S) + offset;
            newInstructions[index].labels.Add(addPositionLabel);

            offset = 4;
            index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ldloc_S) + offset;

            newInstructions[index].labels.Add(skipBypassLabel);

            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                new(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.Subclasses))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SubclassesConfig), nameof(SubclassesConfig.Insurgent))),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Scp079PlayerScript), nameof(Scp079PlayerScript.gameObject))),
                new(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(GameObject) })),
                new(OpCodes.Callvirt, Method(typeof(Insurgent), nameof(Insurgent.Check))),
                new(OpCodes.Brfalse_S, skipBypassLabel),
                new(OpCodes.Ldloc_S, 6),
                new(OpCodes.Ldc_I4_7),
                new(OpCodes.Bne_Un_S, addPositionLabel),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}