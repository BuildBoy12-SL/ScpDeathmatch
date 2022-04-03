// -----------------------------------------------------------------------
// <copyright file="Scavenger.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Items;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomRole(RoleType.ClassD)]
    public class Scavenger : CustomRole
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 106;

        /// <inheritdoc />
        public override RoleType Role { get; set; } = RoleType.ClassD;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Scavenger);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override Vector3 Scale { get; set; } = Vector3.one;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; }

        /// <inheritdoc />
        [YamlIgnore]
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();

        /// <inheritdoc />
        [YamlIgnore]
        public override bool RemovalKillsPlayer { get; set; } = false;

        /// <inheritdoc />
        [YamlIgnore]
        public override bool KeepRoleOnDeath { get; set; } = true;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<string> Inventory { get; set; } = new List<string>();

        /// <inheritdoc />
        [YamlIgnore]
        public override bool KeepInventoryOnSpawn { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether janitor keycards will be replaced with scientist keycards during spawning.
        /// </summary>
        [Description("Whether janitor keycards will be replaced with scientist keycards during spawning.")]
        public bool ReplaceJanitorKeycards { get; set; } = true;

        /// <summary>
        /// Gets or sets the additional ammo the player will spawn with.
        /// </summary>
        [Description("The additional ammo the player will spawn with.")]
        public Dictionary<AmmoType, ushort> AdditionalStartingAmmo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato556, 0 },
            { AmmoType.Nato762, 0 },
            { AmmoType.Nato9, 0 },
            { AmmoType.Ammo12Gauge, 0 },
            { AmmoType.Ammo44Cal, 0 },
        };

        /// <summary>
        /// Gets or sets the item limits for each category.
        /// </summary>
        [Description("The item limits for each category.")]
        public Dictionary<ItemCategory, sbyte> ItemLimits { get; set; } = new Dictionary<ItemCategory, sbyte>
        {
            [ItemCategory.Armor] = -1,
            [ItemCategory.Grenade] = 2,
            [ItemCategory.Keycard] = 3,
            [ItemCategory.Medical] = 3,
            [ItemCategory.MicroHID] = -1,
            [ItemCategory.Radio] = -1,
            [ItemCategory.SCPItem] = 3,
            [ItemCategory.Firearm] = 1,
        };

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            base.UnsubscribeEvents();
        }

        /// <inheritdoc />
        protected override void RoleAdded(Player player)
        {
            ApplyItems(player);
            base.RoleAdded(player);
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!Check(ev.Player) || ev.NewRole == RoleType.None || ev.NewRole == RoleType.Spectator)
                return;

            Timing.CallDelayed(0.5f, () => ApplyItems(ev.Player));
        }

        private void ApplyItems(Player player)
        {
            for (int i = 0; i < player.Ammo.Count; i++)
            {
                ItemType key = player.Ammo.ElementAt(i).Key;
                if (AdditionalStartingAmmo.TryGetValue(key.GetAmmoType(), out ushort ammo))
                    player.Ammo[key] += ammo;
            }

            if (!ReplaceJanitorKeycards)
                return;

            foreach (Item item in player.Items.ToList())
            {
                if (item.Type == ItemType.KeycardJanitor)
                {
                    player.RemoveItem(item);
                    player.AddItem(ItemType.KeycardScientist);
                }
            }
        }
    }
}