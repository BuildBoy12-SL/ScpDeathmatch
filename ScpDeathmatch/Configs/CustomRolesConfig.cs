// -----------------------------------------------------------------------
// <copyright file="CustomRolesConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using Exiled.CustomRoles.API;
    using ScpDeathmatch.CustomRoles;

    /// <summary>
    /// Handles configs related to custom roles.
    /// </summary>
    public class CustomRolesConfig
    {
        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Brute"/> class.
        /// </summary>
        public Brute Brute { get; set; } = new Brute();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Insurgent"/> class.
        /// </summary>
        public Insurgent Insurgent { get; set; } = new Insurgent();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Marksman"/> class.
        /// </summary>
        public Marksman Marksman { get; set; } = new Marksman();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Recon"/> class.
        /// </summary>
        public Recon Recon { get; set; } = new Recon();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Runner"/> class.
        /// </summary>
        public Runner Runner { get; set; } = new Runner();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomRoles.Scavenger"/> class.
        /// </summary>
        public Scavenger Scavenger { get; set; } = new Scavenger();

        /// <summary>
        /// Registers all custom items.
        /// </summary>
        public void Register()
        {
            Brute.Register();
            Insurgent.Register();
            Marksman.Register();
            Recon.Register();
            Runner.Register();
            Scavenger.Register();
        }

        /// <summary>
        /// Unregisters all custom items.
        /// </summary>
        public void Unregister()
        {
            Brute.Unregister();
            Insurgent.Unregister();
            Marksman.Unregister();
            Recon.Unregister();
            Runner.Unregister();
            Scavenger.Unregister();
        }
    }
}