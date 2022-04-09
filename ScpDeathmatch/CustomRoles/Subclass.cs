// -----------------------------------------------------------------------
// <copyright file="Subclass.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using System.Collections.Generic;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public abstract class Subclass : CustomRole
    {
        /// <summary>
        /// Gets or sets the badge to be applied to players that have the role.
        /// </summary>
        public abstract string Badge { get; set; }

        /// <summary>
        /// Gets or sets the color of the <see cref="Badge"/>.
        /// </summary>
        public abstract string BadgeColor { get; set; }

        /// <summary>
        /// Gets or sets the badge to be applied to dead players that have the role.
        /// </summary>
        public virtual string DeadBadge { get; set; } = "Dead";

        /// <summary>
        /// Gets or sets the color of the <see cref="DeadBadge"/>.
        /// </summary>
        public virtual string DeadBadgeColor { get; set; } = "silver";

        /// <inheritdoc />
        [YamlIgnore]
        public override RoleType Role { get; set; } = RoleType.ClassD;

        /// <inheritdoc />
        [YamlIgnore]
        public override SpawnProperties SpawnProperties { get; set; } = new();

        /// <inheritdoc />
        [YamlIgnore]
        public override bool RemovalKillsPlayer { get; set; } = false;

        /// <inheritdoc />
        [YamlIgnore]
        public override bool KeepRoleOnDeath { get; set; } = true;

        /// <inheritdoc />
        [YamlIgnore]
        public override bool KeepInventoryOnSpawn { get; set; } = true;

        /// <inheritdoc />
        public override Vector3 Scale { get; set; } = Vector3.one;

        /// <inheritdoc />
        public override void Init()
        {
            foreach (var ability in CustomAbilities)
                ability.Init();

            base.Init();
        }

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

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnChangingRole(ChangingRoleEventArgs)"/>
        protected virtual void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (Check(ev.Player))
                ev.Player.ReferenceHub.serverRoles.RefreshPermissions();

            Timing.CallDelayed(1f, () =>
            {
                if (Check(ev.Player))
                {
                    ev.Player.ReferenceHub.serverRoles.Network_myText = ev.NewRole == RoleType.Spectator ? DeadBadge : Badge;
                    ev.Player.ReferenceHub.serverRoles.Network_myColor = ev.NewRole == RoleType.Spectator ? DeadBadgeColor : BadgeColor;
                }
            });
        }
    }
}