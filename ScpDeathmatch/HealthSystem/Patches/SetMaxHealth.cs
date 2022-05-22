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

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldarg_1),
                new(OpCodes.Ldc_I4_1),
                new(OpCodes.Newobj, GetDeclaredConstructors(typeof(ChangingMaxHealthEventArgs))[0]),
                new(OpCodes.Stloc_S, ev.LocalIndex),
                new(OpCodes.Ldloc_S, ev.LocalIndex),
                new(OpCodes.Call, Method(typeof(API.Events.Handlers.Player), nameof(API.Events.Handlers.Player.OnChangingMaxHealth))),
                new(OpCodes.Ldloc_S, ev.LocalIndex),
                new(OpCodes.Callvirt, PropertyGetter(typeof(ChangingMaxHealthEventArgs), nameof(ChangingMaxHealthEventArgs.IsAllowed))),
                new(OpCodes.Brfalse_S, returnLabel),
                new(OpCodes.Ldloc_S, ev.LocalIndex),
                new(OpCodes.Callvirt, PropertyGetter(typeof(ChangingMaxHealthEventArgs), nameof(ChangingMaxHealthEventArgs.NewMaxHealth))),
                new(OpCodes.Starg_S, 1),
            });

            newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}