// -----------------------------------------------------------------------
// <copyright file="SubclassesConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using ScpDeathmatch.Subclasses;

    /// <summary>
    /// Handles configs related to custom roles.
    /// </summary>
    public class SubclassesConfig
    {
        private IEnumerable<Subclass> registeredSubclasses;

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Athlete"/> class.
        /// </summary>
        public Athlete Athlete { get; set; } = new();

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

        /*
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Nurse"/> class.
        /// </summary>
        public Nurse Nurse { get; set; } = new();
        */

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Recon"/> class.
        /// </summary>
        public Recon Recon { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Scavenger"/> class.
        /// </summary>
        public Scavenger Scavenger { get; set; } = new();

        /*
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Subclasses.Zombie"/> class.
        /// </summary>
        public Zombie Zombie { get; set; } = new();
        */

        /// <summary>
        /// Registers all subclasses.
        /// </summary>
        public void Register() => registeredSubclasses = Subclass.RegisterSubclasses(this);

        /// <summary>
        /// Unregisters all registered subclasses.
        /// </summary>
        public void Unregister()
        {
            if (registeredSubclasses is null)
                return;

            foreach (Subclass subclass in registeredSubclasses)
                subclass.TryUnregister();
        }
    }
}