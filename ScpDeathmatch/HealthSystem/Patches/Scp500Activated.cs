// -----------------------------------------------------------------------
// <copyright file="Scp500Activated.cs" company="Build">
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
    using PlayerStatsSystem;
    using ScpDeathmatch.Configs;
    using ScpDeathmatch.Models;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Scp500.OnEffectsActivated"/> to fully heal a player and add ahp when they use Scp500.
    /// </summary>
    [HarmonyPatch(typeof(Scp500), nameof(Scp500.OnEffectsActivated))]
    internal static class Scp500Activated
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int offset = -1;
            int index = newInstructions.FindIndex(instruction => instruction.OperandIs(Method(typeof(HealthStat), nameof(HealthStat.ServerHeal), new[] { typeof(float) }))) + offset;

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ItemBase), nameof(ItemBase.Owner))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ItemBase), nameof(ItemBase.Owner))),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(ReferenceHub), nameof(ReferenceHub.characterClassManager))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(CharacterClassManager), nameof(CharacterClassManager.CurRole))),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(Role), nameof(Role.maxHP))),
                new CodeInstruction(OpCodes.Callvirt, PropertySetter(typeof(Player), nameof(Player.MaxHealth))),

                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.MedicalItems))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(MedicalItemsConfig), nameof(MedicalItemsConfig.Scp500Ahp))),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ItemBase), nameof(ItemBase.Owner))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(ConfiguredAhp), nameof(ConfiguredAhp.AddTo), new[] { typeof(Player) })),
            });

            Label skipRegenerationLabel = generator.DefineLabel();

            offset = 1;
            index = newInstructions.FindIndex(instruction => instruction.OperandIs(Method(typeof(HealthStat), nameof(HealthStat.ServerHeal), new[] { typeof(float) }))) + offset;

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.Config))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Config), nameof(Config.MedicalItems))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(MedicalItemsConfig), nameof(MedicalItemsConfig.Scp500Regeneration))),
                new CodeInstruction(OpCodes.Brfalse_S, skipRegenerationLabel),
            });

            index = newInstructions.FindIndex(instruction => instruction.OperandIs(Method(typeof(UsableItem), nameof(UsableItem.ServerAddRegeneration)))) + offset;
            newInstructions[index].labels.Add(skipRegenerationLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}