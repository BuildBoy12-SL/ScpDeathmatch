// -----------------------------------------------------------------------
// <copyright file="UpgradeItems.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.Events.EventArgs;
    using InventorySystem.Items;
    using InventorySystem.Items.Armor;
    using Scp914;
    using Scp914.Processors;

    /// <inheritdoc />
    public class UpgradeItems : IRandomEvent
    {
        /// <inheritdoc />
        public string Name { get; set; } = nameof(UpgradeItems);

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the items that should be upgraded.
        /// </summary>
        [Description("The items that should be upgraded.")]
        public HashSet<ItemType> ToUpgrade { get; set; } = new()
        {
            ItemType.KeycardGuard,
            ItemType.KeycardJanitor,
            ItemType.KeycardO5,
            ItemType.KeycardScientist,
            ItemType.KeycardChaosInsurgency,
            ItemType.KeycardContainmentEngineer,
            ItemType.KeycardFacilityManager,
            ItemType.KeycardResearchCoordinator,
            ItemType.KeycardZoneManager,
            ItemType.KeycardNTFCommander,
            ItemType.KeycardNTFLieutenant,
            ItemType.KeycardNTFOfficer,
        };

        /// <summary>
        /// Gets or sets the type of upgrade to apply.
        /// </summary>
        [Description("The type of upgrade to apply.")]
        public Scp914KnobSetting UpgradeSetting { get; set; } = Scp914KnobSetting.Fine;

        /// <inheritdoc />
        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            foreach (KeyValuePair<ushort, ItemBase> itemPair in ev.Thrower.Inventory.UserInventory.Items.ToList())
            {
                if (ToUpgrade.Contains(itemPair.Value.ItemTypeId) &&
                    Scp914Upgrader.TryGetProcessor(itemPair.Value.ItemTypeId, out Scp914ItemProcessor processor))
                {
                    Action<ItemBase, Scp914KnobSetting> inventoryItemUpgraded = Scp914Upgrader.OnInventoryItemUpgraded;
                    inventoryItemUpgraded?.Invoke(itemPair.Value, UpgradeSetting);
                    processor.OnInventoryItemUpgraded(UpgradeSetting, ev.Thrower.ReferenceHub, itemPair.Key);
                }
            }

            ev.Thrower.Inventory.TryGetBodyArmor(out BodyArmor bodyArmor);
            BodyArmorUtils.RemoveEverythingExceedingLimits(ev.Thrower.Inventory, bodyArmor);
        }
    }
}