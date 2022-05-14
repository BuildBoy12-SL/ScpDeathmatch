// -----------------------------------------------------------------------
// <copyright file="Scavenger.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Scavenger
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using Mirror;
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Subclasses.Subclasses.Scavenger.Abilities;

    /// <inheritdoc />
    public class Scavenger : Subclass
    {
        private readonly SyncList<sbyte> categoryLimits = new();
        private Dictionary<ItemCategory, sbyte> rawCategoryLimits = new()
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
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Scavenger);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override ConfiguredBadge Badge { get; set; } = new(nameof(Scavenger), "tomato");

        /// <summary>
        /// Gets or sets a value indicating whether janitor keycards will be replaced with scientist keycards during spawning.
        /// </summary>
        [Description("Whether janitor keycards will be replaced with scientist keycards during spawning.")]
        public bool ReplaceJanitorKeycards { get; set; } = true;

        /// <summary>
        /// Gets or sets the additional ammo the player will spawn with.
        /// </summary>
        [Description("The additional ammo the player will spawn with.")]
        public Dictionary<AmmoType, ushort> AdditionalStartingAmmo { get; set; } = new()
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
        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new UpgradeItem(),
            new ScavengerAura(),
        };

        /// <inheritdoc />
        protected override void RoleAdded(Player player)
        {
            SyncCategoryLimits(player, categoryLimits);
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
            if (!Check(ev.Player) || ev.NewRole is RoleType.None or RoleType.Spectator)
                return;

            for (int i = 0; i < ev.Ammo.Count; i++)
            {
                ItemType key = ev.Ammo.ElementAt(i).Key;
                if (AdditionalStartingAmmo.TryGetValue(key.GetAmmoType(), out ushort ammo))
                    ev.Ammo[key] += ammo;
            }

            if (!ReplaceJanitorKeycards)
                return;

            for (int i = 0; i < ev.Items.Count; i++)
            {
                if (ev.Items[i] == ItemType.KeycardJanitor)
                    ev.Items[i] = ItemType.KeycardScientist;
            }
        }

        private static void SyncCategoryLimits(Player player, SyncList<sbyte> limits)
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