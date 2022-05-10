// -----------------------------------------------------------------------
// <copyright file="SetMaxHealth.cs" company="Build">
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
    using NorthwoodLib.Pools;
    using ScpDeathmatch.API.Events.EventArgs;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Player.MaxHealth"/> to call <see cref="API.Events.Handlers.Player.OnChangingMaxHealth"/>.
    /// </summary>
    [HarmonyPatch(typeof(Player), nameof(Player.MaxHealth), MethodType.Setter)]
    internal static class SetMaxHealth
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label returnLabel = generator.DefineLabel();
            LocalBuilder ev = generator.DeclareLocal(typeof(ChangingMaxHealthEventArgs));

            newInstructions.InsertRange(0, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(ChangingMaxHealthEventArgs))[0]),
                new CodeInstruction(OpCodes.Stloc_S, ev.LocalIndex),
                new CodeInstruction(OpCodes.Ldloc_S, ev.LocalIndex),
                new CodeInstruction(OpCodes.Call, Method(typeof(API.Events.Handlers.Player), nameof(API.Events.Handlers.Player.OnChangingMaxHealth))),
                new CodeInstruction(OpCodes.Ldloc_S, ev.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ChangingMaxHealthEventArgs), nameof(ChangingMaxHealthEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, returnLabel),
                new CodeInstruction(OpCodes.Ldloc_S, ev.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ChangingMaxHealthEventArgs), nameof(ChangingMaxHealthEventArgs.NewMaxHealth))),
                new CodeInstruction(OpCodes.Starg_S, 1),
            });

            newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}