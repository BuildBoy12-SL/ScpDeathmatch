// -----------------------------------------------------------------------
// <copyright file="VisionControllerPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Exiled.API.Features;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using ScpDeathmatch.Configs;
    using ScpDeathmatch.CustomItems;
    using ScpDeathmatch.Subclasses;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Scp939_VisionController.FixedUpdate"/> to limit the distance of non-scp player's vision.
    /// </summary>
    [HarmonyPatch(typeof(Scp939_VisionController), nameof(Scp939_VisionController.FixedUpdate))]
    internal static class VisionControllerPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label skipOverrideLabel = generator.DefineLabel();

            const int offset = 1;
            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ret) + offset;
            newInstructions[index].labels.Add(skipOverrideLabel);

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.Subclasses))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(SubclassesConfig), nameof(SubclassesConfig.Recon))),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Scp939_VisionController), nameof(Scp939_VisionController.gameObject))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(GameObject) })),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(Recon), nameof(Recon.Check))),
                new CodeInstruction(OpCodes.Brfalse_S, skipOverrideLabel),

                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.CustomItems))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(CustomItemsConfig), nameof(CustomItemsConfig.ReconSwitch))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ReconSwitch), nameof(ReconSwitch.MaximumDistance))),
                new CodeInstruction(OpCodes.Stsfld, Field(typeof(Scp939_VisionController), nameof(Scp939_VisionController.noise))),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}