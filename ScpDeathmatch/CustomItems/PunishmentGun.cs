// -----------------------------------------------------------------------
// <copyright file="PunishmentGun.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems
{
    using System.ComponentModel;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomItem(ItemType.GunRevolver)]
    public class PunishmentGun : CustomWeapon
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 129;

        /// <inheritdoc />
        public override string Name { get; set; } = "Punishment Gun";

        /// <inheritdoc />
        public override string Description { get; set; } = "Bans a shot player with the specified reason";

        /// <inheritdoc />
        public override float Weight { get; set; }

        /// <inheritdoc/>
        public override SpawnProperties SpawnProperties { get; set; } = new();

        /// <inheritdoc/>
        public override float Damage { get; set; }

        /// <summary>
        /// Gets or sets the duration to use when banning a player shot with this gun.
        /// </summary>
        [Description("The duration to use when banning a player shot with this gun.")]
        public int BanDuration { get; set; } = int.MaxValue;

        /// <summary>
        /// Gets or sets the reason to use when banning a player shot with this gun.
        /// </summary>
        [Description("The reason to use when banning a player shot with this gun.")]
        public string BanReason { get; set; } = "You've been shot through the 4th wall";

        /// <inheritdoc />
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.GunRevolver;

        /// <inheritdoc />
        protected override void OnShot(ShotEventArgs ev)
        {
            ev.Target.Ban(BanDuration, BanReason, ev.Shooter.Nickname);
        }
    }
}