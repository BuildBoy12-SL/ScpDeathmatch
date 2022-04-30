// -----------------------------------------------------------------------
// <copyright file="SpawnGrenade.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;

    /// <inheritdoc />
    public class SpawnGrenade : IRandomEvent
    {
        /// <inheritdoc />
        public string Name { get; set; } = nameof(SpawnGrenade);

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public float Weight { get; set; } = -0.8f;

        /// <summary>
        /// Gets or sets the fuse time.
        /// </summary>
        public float FuseTime { get; set; } = 2f;

        /// <inheritdoc />
        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            ExplosiveGrenade explosiveGrenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE, ev.Thrower);
            explosiveGrenade.FuseTime = FuseTime;
            explosiveGrenade.SpawnActive(ev.Grenade.transform.position, ev.Thrower);
        }
    }
}