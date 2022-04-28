// -----------------------------------------------------------------------
// <copyright file="AdrenalineActivated.cs" company="Build">
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
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using HarmonyLib;
    using InventorySystem.Items.Usables;
    using NorthwoodLib.Pools;
    using ScpDeathmatch.Configs;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Medkit.OnEffectsActivated"/> to remove the artificial health gain.
    /// </summary>
    [HarmonyPatch(typeof(Adrenaline), nameof(Adrenaline.OnEffectsActivated))]
    internal static class AdrenalineActivated
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label skipAhpLabel = generator.DefineLabel();

            const int offset = 1;
            int index = newInstructions.FindIndex(instruction => instruction.OperandIs(Method(typeof(FirstPersonController), nameof(FirstPersonController.ModifyStamina)))) + offset;
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Adrenaline), nameof(Adrenaline.Owner))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Call, Method(typeof(AdrenalineActivated), nameof(EnableMovementBoost))),

                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.MedicalItems))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(MedicalItemsConfig), nameof(MedicalItemsConfig.AdrenalineAhp))),
                new CodeInstruction(OpCodes.Brfalse_S, skipAhpLabel),
            });

            index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Pop) + offset;
            newInstructions[index].labels.Add(skipAhpLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }

        private static void EnableMovementBoost(Player player)
        {
            player.EnableEffect(EffectType.MovementBoost, 8f, true);
            player.ChangeEffectIntensity(EffectType.MovementBoost, Plugin.Instance.Config.MedicalItems.AdrenalineMovementBoost);
        }
    }
}