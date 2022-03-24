// -----------------------------------------------------------------------
// <copyright file="Runner.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using Exiled.CustomRoles.API.Features;

    public class Runner : CustomRole
    {
        public override uint Id { get; set; } = 105;

        public override RoleType Role { get; set; }

        public override int MaxHealth { get; set; }

        public override string Name { get; set; }

        public override string Description { get; set; }

        public override string CustomInfo { get; set; }
    }
}