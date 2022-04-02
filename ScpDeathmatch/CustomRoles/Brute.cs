// -----------------------------------------------------------------------
// <copyright file="Brute.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using Exiled.API.Features.Attributes;
    using Exiled.CustomRoles.API.Features;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomRole(RoleType.ClassD)]
    public class Brute : CustomRole
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 101;

        /// <inheritdoc />
        [YamlIgnore]
        public override RoleType Role { get; set; } = RoleType.ClassD;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc/>
        public override string Name { get; set; } = nameof(Brute);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }
    }
}