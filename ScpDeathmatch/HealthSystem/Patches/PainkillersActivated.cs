﻿// -----------------------------------------------------------------------
// <copyright file="PainkillersActivated.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem.Items;
    using InventorySystem.Items.Usables;
    using NorthwoodLib.Pools;
    using ScpDeathmatch.Configs;
    using ScpDeathmatch.Models;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Painkillers.OnEffectsActivated"/> to fully heal a player when they use a medkit.
    /// </summary>
    [HarmonyPatch(typeof(Painkillers), nameof(Painkillers.OnEffectsActivated))]
    internal static class PainkillersActivated
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label skipRegenerationLabel = generator.DefineLabel();

            newInstructions.InsertRange(0, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.MedicalItems))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(MedicalItemsConfig), nameof(MedicalItemsConfig.PainkillersAhp))),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ItemBase), nameof(ItemBase.Owner))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(ConfiguredAhp), nameof(ConfiguredAhp.AddTo), new[] { typeof(Player) })),

                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.MedicalItems))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(MedicalItemsConfig), nameof(MedicalItemsConfig.PainkillersRegeneration))),
                new CodeInstruction(OpCodes.Brfalse_S, skipRegenerationLabel),
            });

            const int offset = 1;
            int index = newInstructions.FindIndex(instruction => instruction.OperandIs(Method(typeof(UsableItem), nameof(UsableItem.ServerAddRegeneration)))) + offset;
            newInstructions[index].labels.Add(skipRegenerationLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}