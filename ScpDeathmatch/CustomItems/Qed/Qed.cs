// -----------------------------------------------------------------------
// <copyright file="Qed.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.Events.EventArgs;

namespace ScpDeathmatch.CustomItems.Qed
{
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;

    /// <inheritdoc />
    public class Qed : CustomGrenade
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 125;

        /// <inheritdoc />
        public override string Name { get; set; } = "QED";

        /// <inheritdoc />
        public override string Description { get; set; } = "A device that will initiate a random event on detonation";

        /// <inheritdoc />
        public override float Weight { get; set; } = 0f;

        /// <inheritdoc />
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();

        /// <inheritdoc />
        public override bool ExplodeOnCollision { get; set; } = false;

        /// <inheritdoc />
        public override float FuseTime { get; set; } = 3f;

        /// <inheritdoc />
        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            base.OnExploding(ev);
        }
    }
}