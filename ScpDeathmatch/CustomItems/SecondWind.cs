// -----------------------------------------------------------------------
// <copyright file="SecondWind.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Items;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using Footprinting;
    using InventorySystem.Items.ThrowableProjectiles;
    using InventorySystem.Items.Usables.Scp330;
    using PlayerStatsSystem;
    using Scp914;
    using ScpDeathmatch.Enums;
    using ScpDeathmatch.Managers;
    using ScpDeathmatch.Models;
    using UnityEngine;
    using Utils.Networking;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomItem(ItemType.Radio)]
    public class SecondWind : CustomItem
    {
        private Player lastDetonator;

        /// <inheritdoc />
        public override uint Id { get; set; } = 123;

        /// <inheritdoc />
        public override string Name { get; set; } = "Second Wind";

        /// <inheritdoc />
        public override string Description { get; set; } = "Blow up and respawn if you take another player's life";

        /// <inheritdoc />
        public override float Weight { get; set; }

        /// <inheritdoc />
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.Radio;

        /// <summary>
        /// Gets or sets a value indicating whether a kill is required to respawn on use.
        /// </summary>
        [Description("Whether a kill is required to respawn on use.")]
        public bool KillRequired { get; set; } = true;

        /// <summary>
        /// Gets or sets the method of teleportation to use if the explosion kills another player.
        /// </summary>
        [Description("The method of teleportation to use to respawn the user. Available: None, Zone, Role")]
        public TeleportType TeleportType { get; set; } = TeleportType.Zone;

        /// <summary>
        /// Gets or sets a collection of item-chance pairs that can upgrade into the second wind item.
        /// </summary>
        [Description("A collection of item-chance pairs that can upgrade into the second wind item.")]
        public Dictionary<ItemType, UpgradeSetting> UpgradeSettings { get; set; } = new()
        {
            {
                ItemType.Coin, new UpgradeSetting(oneToOne: 100)
            },
        };

        /// <inheritdoc />
        public override SpawnProperties SpawnProperties { get; set; } = new();

        /// <summary>
        /// Detonates the second wind.
        /// </summary>
        /// <param name="player">The player that detonated the item.</param>
        public void Detonate(Player player)
        {
            lastDetonator = player;
            if (!CandyPink.TryGetGrenade(out ExplosionGrenade grenade))
                return;

            new CandyPink.CandyExplosionMessage { Origin = player.Position }.SendToAuthenticated();
            ExplosionGrenade.Explode(new Footprint(player.ReferenceHub), player.Position, grenade);
        }

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Map.ExplodingGrenade += OnExplodingGrenade;
            Exiled.Events.Handlers.Scp914.UpgradingItem += OnUpgradingItem;
            Exiled.Events.Handlers.Scp914.UpgradingPlayer += OnUpgradingPlayer;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Map.ExplodingGrenade -= OnExplodingGrenade;
            Exiled.Events.Handlers.Scp914.UpgradingItem -= OnUpgradingItem;
            Exiled.Events.Handlers.Scp914.UpgradingPlayer -= OnUpgradingPlayer;
            base.UnsubscribeEvents();
        }

        private void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            if (ev.Thrower != lastDetonator || ev.GrenadeType != GrenadeType.FragGrenade)
                return;

            lastDetonator = null;
            RespawnManager.Add(new Respawner(ev.Thrower, () => !KillRequired || ev.TargetsToAffect.Count > 0, TeleportType));
            foreach (Player player in ev.TargetsToAffect)
                player.Hurt(new UniversalDamageHandler(-1f, DeathTranslations.Explosion, DamageHandlerBase.CassieAnnouncement.Default));
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

            if (!TryGet(player, out IEnumerable<CustomItem> _))
                Give(player);
            else
                Spawn(outputPosition, (Player)null);

            foreach (Item playerItem in player.Items.ToList())
            {
                if (playerItem is Radio && !Check(playerItem))
                {
                    player.RemoveItem(playerItem);
                    playerItem.Spawn(outputPosition);
                }
            }
        }
    }
}