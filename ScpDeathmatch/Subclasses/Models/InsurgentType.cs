// -----------------------------------------------------------------------
// <copyright file="InsurgentType.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Models
{
    using System.Collections.Generic;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles the type of insurgent.
    /// </summary>
    public class InsurgentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsurgentType"/> class.
        /// </summary>
        public InsurgentType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsurgentType"/> class.
        /// </summary>
        /// <param name="role"><inheritdoc cref="Role"/></param>
        /// <param name="badge"><inheritdoc cref="Badge"/></param>
        /// <param name="inventory"><inheritdoc cref="Inventory"/></param>
        public InsurgentType(RoleType role, Badge badge, List<string> inventory = null)
        {
            Role = role;
            Badge = badge;
            Inventory = inventory;
        }

        /// <summary>
        /// Gets or sets the corresponding role.
        /// </summary>
        public RoleType Role { get; set; }

        /// <summary>
        /// Gets or sets the badge to use.
        /// </summary>
        public Badge Badge { get; set; }

        /// <summary>
        /// Gets or sets the items to give.
        /// </summary>
        public List<string> Inventory { get; set; }
    }
}