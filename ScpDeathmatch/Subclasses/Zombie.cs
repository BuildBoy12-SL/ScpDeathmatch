// -----------------------------------------------------------------------
// <copyright file="Zombie.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses
{
    using ScpDeathmatch.Models;

    /// <inheritdoc />
    public class Zombie : Subclass
    {
        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 300;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Zombie);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override ConfiguredBadge Badge { get; set; } = new(nameof(Zombie), "carmine");
    }
}