// -----------------------------------------------------------------------
// <copyright file="UpgradeItem.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Scavenger.Abilities
{
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using InventorySystem.Items.Armor;
    using Scp914;
    using Scp914.Processors;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class UpgradeItem : ActiveAbility
    {
        /// <inheritdoc />
        public override string Name { get; set; } = "Upgrade Item";

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        [YamlIgnore]
        public override float Duration { get; set; }

        /// <inheritdoc />
        public override float Cooldown { get; set; } = 90f;

        /// <summary>
        /// Gets or sets the type of upgrade to apply.
        /// </summary>
        [Description("The type of upgrade to apply.")]
        public Scp914KnobSetting UpgradeSetting { get; set; } = Scp914KnobSetting.Fine;

        /// <inheritdoc />
        public override bool CanUseAbility(Player player, out string response)
        {
            if (player.CurrentItem is null)
            {
                response = "You don't have an item equipped!";
                return false;
            }

            if (!Scp914Upgrader.TryGetProcessor(player.CurrentItem.Type, out _))
            {
                response = "Your current item cannot be upgraded!";
                return false;
            }

            return base.CanUseAbility(player, out response);
        }

        /// <inheritdoc />
        protected override void AbilityUsed(Player player)
        {
            if (!Scp914Upgrader.TryGetProcessor(player.CurrentItem.Type, out Scp914ItemProcessor processor))
                return;

            processor.OnInventoryItemUpgraded(UpgradeSetting, player.ReferenceHub, player.CurrentItem.Serial);
            player.Inventory.TryGetBodyArmor(out BodyArmor bodyArmor);
            BodyArmorUtils.RemoveEverythingExceedingLimits(player.Inventory, bodyArmor);
        }
    }
}