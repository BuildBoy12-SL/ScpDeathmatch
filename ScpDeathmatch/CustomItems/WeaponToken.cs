// -----------------------------------------------------------------------
// <copyright file="WeaponToken.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Items;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using Scp914;
    using ScpDeathmatch.API.Extensions;
    using ScpDeathmatch.Models;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomItem(ItemType.Coin)]
    public class WeaponToken : CustomItem
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 124;

        /// <inheritdoc />
        public override string Name { get; set; } = "Weapon Token";

        /// <inheritdoc />
        public override string Description { get; set; } = "To be thrown into the HczArmory pit for a random item";

        /// <inheritdoc />
        public override float Weight { get; set; }

        /// <inheritdoc />
        public override SpawnProperties SpawnProperties { get; set; } = new();

        /// <inheritdoc />
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.Coin;

        /// <summary>
        /// Gets or sets a collection of item-chance pairs that can upgrade into a weapon token.
        /// </summary>
        [Description("A collection of item-chance pairs that can upgrade into a weapon token.")]
        public Dictionary<ItemType, UpgradeSetting> UpgradeSettings { get; set; } = new()
        {
            {
                ItemType.KeycardJanitor, new UpgradeSetting
                {
                    Chances = new Dictionary<Scp914KnobSetting, int>
                    {
                        { Scp914KnobSetting.Rough, 0 },
                        { Scp914KnobSetting.Coarse, 0 },
                        { Scp914KnobSetting.OneToOne, 100 },
                        { Scp914KnobSetting.Fine, 0 },
                        { Scp914KnobSetting.VeryFine, 0 },
                    },
                }
            },
        };

        /// <summary>
        /// Gets or sets the list of possible rewards.
        /// </summary>
        [Description("The list of possible rewards. Accepts custom item names.")]
        public List<string> PossibleRewards { get; set; } = new()
        {
            $"{ItemType.GunRevolver}",
            $"{ItemType.MicroHID}",
            "BigIron",
        };

        /// <summary>
        /// Gets or sets the message to send a player when they receive their item.
        /// </summary>
        [Description("The message to send a player when they receive their item.")]
        public string GaveItem { get; set; } = "Gave you a {0} in exchange for your weapon token.";

        /// <summary>
        /// Gives a player a random reward from the <see cref="PossibleRewards"/> collection.
        /// </summary>
        /// <param name="player">The player to give the item to.</param>
        public void GiveRandom(Player player)
        {
            if (PossibleRewards is null || PossibleRewards.Count == 0)
                return;

            string name = PossibleRewards.Random();
            if (TryGive(player, name, false))
            {
                player.ShowHint(string.Format(GaveItem, name));
                return;
            }

            if (Enum.TryParse(name, true, out ItemType itemType))
            {
                player.AddItem(itemType);
                player.ShowHint(string.Format(GaveItem, name));
                return;
            }

            Log.Warn($"Failed to give a reward for a weapon token. '{name}' is not a valid custom item or item type.");
        }

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingItem += OnUpgradingItem;
            Exiled.Events.Handlers.Scp914.UpgradingPlayer += OnUpgradingPlayer;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingItem -= OnUpgradingItem;
            Exiled.Events.Handlers.Scp914.UpgradingPlayer -= OnUpgradingPlayer;
            base.UnsubscribeEvents();
        }

        private void OnUpgradingItem(UpgradingItemEventArgs ev)
        {
            if (TryGet(ev.Item, out _))
                return;

            if (!UpgradeSettings.TryGetValue(ev.Item.Type, out UpgradeSetting chance) || !chance.Check(ev.KnobSetting))
                return;

            ev.Item.Destroy();
            Spawn(ev.OutputPosition, (Player)null);
        }

        private void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if (ev.HeldOnly)
            {
                UpgradeItem(ev.Player.CurrentItem, ev.KnobSetting, ev.Player, ev.OutputPosition);
                return;
            }

            foreach (Item item in ev.Player.Items.ToList())
                UpgradeItem(item, ev.KnobSetting, ev.Player, ev.OutputPosition);
        }

        private void UpgradeItem(Item item, Scp914KnobSetting setting, Player player, Vector3 outputPosition)
        {
            if (item is null ||
                TryGet(item, out _) ||
                !UpgradeSettings.TryGetValue(item.Type, out UpgradeSetting chance) ||
                !chance.Check(setting))
                return;

            player.RemoveItem(item);
            Give(player);
        }
    }
}