// -----------------------------------------------------------------------
// <copyright file="StartUsingConsumable.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Nurse.Patches
{
    using System.Collections.Generic;
    using HarmonyLib;
    using InventorySystem.Items.Usables;
    using NorthwoodLib.Pools;

    /// <summary>
    /// Patches <see cref="Consumable.OnUsingStarted"/> to allow the <see cref="Nurse"/> subclass to instantly consume medical items.
    /// </summary>
    [HarmonyPatch(typeof(Consumable), nameof(Consumable.OnUsingStarted))]
    internal static class StartUsingConsumable
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}