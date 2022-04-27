// -----------------------------------------------------------------------
// <copyright file="StubbyRevolver.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems
{
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomItem(ItemType.GunRevolver)]
    public class StubbyRevolver : CustomWeapon
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 127;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(StubbyRevolver);

        /// <inheritdoc />
        public override string Description { get; set; } = "A revolver that can only hold one shot.";

        /// <inheritdoc />
        public override float Weight { get; set; } = 0;

        /// <inheritdoc />
        public override SpawnProperties SpawnProperties { get; set; } = new();

        /// <inheritdoc />
        public override float Damage { get; set; }

        /// <inheritdoc />
        public override byte ClipSize { get; set; } = 1;

        /// <inheritdoc />
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.GunRevolver;
    }
}