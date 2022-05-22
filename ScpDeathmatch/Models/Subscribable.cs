// -----------------------------------------------------------------------
// <copyright file="Subscribable.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    /// <summary>
    /// Defines the contract for subscribable objects.
    /// </summary>
    public abstract class Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Subscribable"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="ScpDeathmatch.Plugin"/> class.</param>
        protected Subscribable(Plugin plugin) => Plugin = plugin;

        /// <summary>
        /// Gets the plugin class instance.
        /// </summary>
        protected Plugin Plugin { get; }

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public abstract void Subscribe();

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public abstract void Unsubscribe();
    }
}