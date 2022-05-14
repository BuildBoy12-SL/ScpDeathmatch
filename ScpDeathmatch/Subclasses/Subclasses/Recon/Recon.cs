// -----------------------------------------------------------------------
// <copyright file="Recon.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Recon
{
    using System.Collections.Generic;
    using Exiled.CustomRoles.API.Features;
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Subclasses.Subclasses.Recon.Abilities;

    /// <inheritdoc />
    public class Recon : Subclass
    {
        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Recon);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override ConfiguredBadge Badge { get; set; } = new(nameof(Recon), "mint");

        /// <inheritdoc />
        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new PlayerDetection(),
            new StatusAffliction(),
            new ToggleGoggles(),
        };
    }
}