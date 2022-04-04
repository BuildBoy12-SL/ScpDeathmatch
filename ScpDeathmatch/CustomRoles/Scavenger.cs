// -----------------------------------------------------------------------
// <copyright file="Scavenger.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Items;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using Mirror;
    using ScpDeathmatch.Abilities;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomRole(RoleType.ClassD)]
    public class Scavenger : Subclass
    {
        private readonly SyncList<sbyte> categoryLimits = new SyncList<sbyte>();
        private Dictionary<ItemCategory, sbyte> rawCategoryLimits = new Dictionary<ItemCategory, sbyte>
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
        public override uint Id { get; set; } = 106;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Scavenger);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override string Badge { get; set; } = nameof(Scavenger);

        /// <inheritdoc />
        public override string BadgeColor { get; set; } = "tomato";

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
        public Dictionary<ItemCategory, sbyte> ItemLimits
        {
            get => rawCategoryLimits;
            set
            {
                rawCategoryLimits = value;
                categoryLimits.Clear();
                for (int index = 0; Enum.IsDefined(typeof(ItemCategory), (ItemCategory)index); ++index)
                {
                    ItemCategory key = (ItemCategory)index;
                    if (rawCategoryLimits.TryGetValue(key, out sbyte def) && def >= 0)
                        categoryLimits.Add(def);
                }
            }
        }

        /// <inheritdoc />
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
        {
            new ScavengerAura(),
        };

        /// <inheritdoc />
        protected override void RoleAdded(Player player)
        {
            SyncCategoryLimits(player, categoryLimits);
            ApplyItems(player);
            base.RoleAdded(player);
        }

        /// <inheritdoc />
        protected override void RoleRemoved(Player player)
        {
            SyncCategoryLimits(player, ServerConfigSynchronizer.Singleton.CategoryLimits);
            base.RoleRemoved(player);
        }

        /// <inheritdoc />
        protected override void OnChangingRole(ChangingRoleEventArgs ev)
        {
            base.OnChangingRole(ev);
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

        private void SyncCategoryLimits(Player player, SyncList<sbyte> limits)
        {
            MirrorExtensions.SendFakeSyncObject(player, ServerConfigSynchronizer.Singleton.netIdentity, typeof(ServerConfigSynchronizer), (writer) =>
            {
                writer.WriteUInt64(1ul);
                writer.WriteUInt32((uint)limits.Count);
                for (int i = 0; i < limits.Count; i++)
                {
                    writer.WriteByte((byte)SyncList<byte>.Operation.OP_SET);
                    writer.WriteUInt32((uint)i);
                    writer.WriteSByte(limits[i]);
                }
            });
        }
    }
}