// -----------------------------------------------------------------------
// <copyright file="ValidateDisarmPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1313
    using HarmonyLib;
    using InventorySystem;
    using InventorySystem.Disarming;

    /// <summary>
    /// Patches <see cref="DisarmedPlayers.ValidateEntry"/> to implement configs from <see cref="Config"/>.
    /// </summary>
    [HarmonyPatch(typeof(DisarmedPlayers), nameof(DisarmedPlayers.ValidateEntry))]
    internal static class ValidateDisarmPatch
    {
        private static bool Prefix(DisarmedPlayers.DisarmedEntry entry, ref bool __result)
        {
            if (entry.Disarmer == 0U)
            {
                __result = true;
                return false;
            }

            if (!ReferenceHub.TryGetHubNetID(entry.DisarmedPlayer, out ReferenceHub disarmedHub) ||
                !ReferenceHub.TryGetHubNetID(entry.Disarmer, out ReferenceHub disarmerHub) ||
                !disarmedHub.characterClassManager.IsHuman() || !disarmerHub.characterClassManager.IsHuman() ||
                disarmedHub.characterClassManager.Faction == disarmerHub.characterClassManager.Faction ||
                (Plugin.Instance.Config.Disarming.DisarmAtDistance && (disarmedHub.transform.position - disarmerHub.transform.position).sqrMagnitude > 8100.0))
            {
                __result = false;
                return false;
            }

            if (Plugin.Instance.Config.Disarming.DropItems)
                disarmedHub.inventory.ServerDropEverything();

            __result = true;
            return false;
        }
    }
}