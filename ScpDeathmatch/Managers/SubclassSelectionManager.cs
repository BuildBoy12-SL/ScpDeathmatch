// -----------------------------------------------------------------------
// <copyright file="SubclassSelectionManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    /// <summary>
    /// Manages the selection of subclasses at round start.
    /// </summary>
    public class SubclassSelectionManager
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubclassSelectionManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public SubclassSelectionManager(Plugin plugin) => this.plugin = plugin;
    }
}