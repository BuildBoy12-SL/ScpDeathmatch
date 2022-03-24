// -----------------------------------------------------------------------
// <copyright file="Insurgent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using Exiled.CustomRoles.API.Features;

    public class Insurgent : CustomRole
    {
        public override uint Id { get; set; } = 102;

        public override RoleType Role { get; set; } = RoleType.ClassD;

        public override int MaxHealth { get; set; } = 100;

        public override string Name { get; set; } = nameof(Insurgent);

        public override string Description { get; set; }

        public override string CustomInfo { get; set; }
    }
}