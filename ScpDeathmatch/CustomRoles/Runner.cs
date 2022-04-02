// -----------------------------------------------------------------------
// <copyright file="Runner.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using Exiled.API.Features.Attributes;
    using Exiled.CustomRoles.API.Features;

    /// <inheritdoc />
    [CustomRole(RoleType.ClassD)]
    public class Runner : CustomRole
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 105;

        /// <inheritdoc />
        public override RoleType Role { get; set; } = RoleType.ClassD;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Runner);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }
    }
}