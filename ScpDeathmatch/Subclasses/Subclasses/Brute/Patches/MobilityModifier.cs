﻿// -----------------------------------------------------------------------
// <copyright file="MobilityModifier.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Brute.Patches
{
#pragma warning disable SA1118
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem.Items;
    using InventorySystem.Items.Firearms;
    using NorthwoodLib.Pools;
    using ScpDeathmatch.API.Interfaces;
    using ScpDeathmatch.Configs;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches all <see cref="IMobilityModifyingItem.StaminaUsageMultiplier"/> and <see cref="IMobilityModifyingItem.MovementSpeedMultiplier"/> instances.
    /// </summary>
    internal class MobilityModifier : IManualPatch
    {
        /// <inheritdoc/>
        public void Patch(Harmony harmony)
        {
            Type firearmType = typeof(Firearm);
            PropertyInfo staminaUsageMultiplier = firearmType.GetProperty("StaminaUsageMultiplier", BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo movementSpeedMultiplier = firearmType.GetProperty("MovementSpeedMultiplier", BindingFlags.Public | BindingFlags.Instance);

            if (staminaUsageMultiplier is null || movementSpeedMultiplier is null)
                return;

            HarmonyMethod transpiler = new HarmonyMethod(typeof(MobilityModifier).GetMethod(nameof(Transpiler), BindingFlags.NonPublic | BindingFlags.Static));
            harmony.Patch(staminaUsageMultiplier.GetMethod, transpiler: transpiler);
            harmony.Patch(movementSpeedMultiplier.GetMethod, transpiler: transpiler);
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label baseLogicLabel = generator.DefineLabel();
            newInstructions[0].labels.Add(baseLogicLabel);

            LocalBuilder brute = generator.DeclareLocal(typeof(Brute));

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.Subclasses))),
                new(OpCodes.Callvirt, PropertyGetter(typeof(SubclassesConfig), nameof(SubclassesConfig.Brute))),
                new(OpCodes.Stloc_S, brute.LocalIndex),

                new(OpCodes.Ldloc_S, brute.LocalIndex),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Firearm), nameof(Firearm.Owner))),
                new(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new(OpCodes.Callvirt, Method(typeof(Brute), nameof(Brute.Check))),
                new(OpCodes.Brfalse_S, baseLogicLabel),

                new(OpCodes.Ldloc_S, brute.LocalIndex),
                new(OpCodes.Callvirt, PropertyGetter(typeof(Brute), nameof(Brute.IgnoreItemWeight))),
                new(OpCodes.Brfalse_S, baseLogicLabel),

                new(OpCodes.Ldc_R4, 1f),
                new(OpCodes.Ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}