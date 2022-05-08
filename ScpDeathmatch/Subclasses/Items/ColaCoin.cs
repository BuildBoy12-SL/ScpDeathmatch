// -----------------------------------------------------------------------
// <copyright file="ColaCoin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Items
{
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Items;
    using Exiled.API.Features.Spawn;
    using Exiled.Events.EventArgs;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomItem(ItemType.Coin)]
    public class ColaCoin : SubclassItem
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 128;

        /// <inheritdoc />
        public override string Name { get; set; } = "Cola Coin";

        /// <inheritdoc />
        public override string Description { get; set; } = "When held by an athlete in 914 it will always transform into a cola.";

        /// <inheritdoc />
        public override float Weight { get; set; }

        /// <inheritdoc />
        public override SpawnProperties SpawnProperties { get; set; } = new();

        /// <inheritdoc />
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.Coin;

        /// <summary>
        /// Gets or sets the amount of colas to grant when upgraded.
        /// </summary>
        [Description("The amount of colas to grant when upgraded.")]
        public int ColaAmount { get; set; } = 1;

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
            if (Check(ev.Item))
                ev.IsAllowed = false;
        }

        private void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if (ev.HeldOnly)
            {
                UpgradeItem(ev.Player.CurrentItem, ev.Player);
                return;
            }

            foreach (Item item in ev.Player.Items.ToList())
                UpgradeItem(item, ev.Player);
        }

        private void UpgradeItem(Item item, Player player)
        {
            if (item is null || !Check(item) || !Plugin.Instance.Config.Subclasses.Athlete.Check(player))
                return;

            player.RemoveItem(item);
            for (int i = 0; i < ColaAmount; i++)
            {
                if (player.Items.Count >= 8)
                {
                    Item.Create(ItemType.SCP207).Spawn(player.Position + Vector3.up);
                    continue;
                }

                player.AddItem(ItemType.SCP207);
            }
        }
    }
}