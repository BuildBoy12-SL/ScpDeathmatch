// -----------------------------------------------------------------------
// <copyright file="SubclassesConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using ScpDeathmatch.CustomRoles;

    /// <summary>
    /// Handles configs related to custom roles.
    /// </summary>
    public class SubclassesConfig
    {
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Brute"/> class.
        /// </summary>
        public Brute Brute { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Insurgent"/> class.
        /// </summary>
        public Insurgent Insurgent { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Marksman"/> class.
        /// </summary>
        public Marksman Marksman { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Recon"/> class.
        /// </summary>
        public Recon Recon { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Runner"/> class.
        /// </summary>
        public Runner Runner { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Scavenger"/> class.
        /// </summary>
        public Scavenger Scavenger { get; set; } = new();

        /// <summary>
        /// Registers all custom items.
        /// </summary>
        public void Register() => Subclass.RegisterSubclasses(this);

        /// <summary>
        /// Unregisters all custom items.
        /// </summary>
        public void Unregister() => Subclass.UnregisterSubclasses();
    }
}