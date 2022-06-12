// -----------------------------------------------------------------------
// <copyright file="ActivatingConsumableEffects.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.API.Events.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem.Items.Usables;
    using NorthwoodLib.Pools;
    using ScpDeathmatch.API.Events.EventArgs;
    using ScpDeathmatch.API.Interfaces;
    using static HarmonyLib.AccessTools;

    /// <inheritdoc />
    internal class ActivatingConsumableEffects : IManualPatch
    {
        /// <inheritdoc/>
        public void Patch(Harmony harmony)
        {
            HarmonyMethod transpiler = new HarmonyMethod(typeof(ActivatingConsumableEffects).GetMethod(nameof(Transpiler), BindingFlags.NonPublic | BindingFlags.Static));
            foreach (var type in typeof(Consumable).Assembly.GetTypes())
            {
                if (!type.IsSubclassOf(typeof(Consumable)))
                    continue;

                MethodInfo methodInfo = type.GetMethod("OnEffectsActivated", BindingFlags.NonPublic | BindingFlags.Instance);
                if (methodInfo is not null)
                    harmony.Patch(methodInfo, transpiler: transpiler);
            }
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label returnLabel = generator.DefineLabel();

            newInstructions.InsertRange(0, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Consumable), nameof(Consumable.Owner))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(ActivatingConsumableEffectsEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnActivatingConsumableEffects))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ActivatingConsumableEffectsEventArgs), nameof(ActivatingConsumableEffectsEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, returnLabel),
            });

            newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}