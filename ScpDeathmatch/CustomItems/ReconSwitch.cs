// -----------------------------------------------------------------------
// <copyright file="ReconSwitch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems
{
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;

    public class ReconSwitch : CustomItem
    {
        public override uint Id { get; set; } = 122;

        public override string Name { get; set; }

        public override string Description { get; set; }

        public override float Weight { get; set; }

        public override SpawnProperties SpawnProperties { get; set; }
    }
}