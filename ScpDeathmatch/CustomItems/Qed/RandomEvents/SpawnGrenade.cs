// -----------------------------------------------------------------------
// <copyright file="SpawnGrenade.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using System.ComponentModel;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.CustomItems.Qed.Enums;

    /// <inheritdoc />
    public class SpawnGrenade : IRandomEvent
    {
        /// <inheritdoc />
        public string Name { get; set; } = nameof(SpawnGrenade);

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public RangeType Range { get; set; } = RangeType.Far;

        /// <summary>
        /// Gets or sets the amount of grenades to spawn.
        /// </summary>
        [Description("The amount of grenades to spawn.")]
        public int Amount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the fuse time of the spawned grenade.
        /// </summary>
        [Description("The fuse time of the spawned grenade.")]
        public float FuseTime { get; set; } = 2f;

        /// <inheritdoc />
        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            ExplosiveGrenade explosiveGrenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE, ev.Thrower);
            explosiveGrenade.FuseTime = FuseTime;
            for (int i = 0; i < Amount; i++)
                explosiveGrenade.SpawnActive(ev.Grenade.transform.position, ev.Thrower);
        }
    }
}