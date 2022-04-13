// -----------------------------------------------------------------------
// <copyright file="SubclassesConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using ScpDeathmatch.Subclasses;

    /// <summary>
    /// Handles configs related to custom roles.
    /// </summary>
    public class SubclassesConfig
    {
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Brute"/> class.
        /// </summary>
        public Brute Brute { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Insurgent"/> class.
        /// </summary>
        public Insurgent Insurgent { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Marksman"/> class.
        /// </summary>
        public Marksman Marksman { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Recon"/> class.
        /// </summary>
        public Recon Recon { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Runner"/> class.
        /// </summary>
        public Runner Runner { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Scavenger"/> class.
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