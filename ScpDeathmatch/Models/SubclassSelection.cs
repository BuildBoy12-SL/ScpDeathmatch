// -----------------------------------------------------------------------
// <copyright file="SubclassSelection.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using Exiled.CustomRoles.API.Features;

    /// <summary>
    /// Represents a selection of a subclass.
    /// </summary>
    public class SubclassSelection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubclassSelection"/> class.
        /// </summary>
        public SubclassSelection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubclassSelection"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc cref="Name"/></param>
        /// <param name="message"><inheritdoc cref="Message"/></param>
        public SubclassSelection(string name, string message)
        {
            Name = name;
            Message = message;
        }

        /// <summary>
        /// Gets or sets the name of the subclass.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the message to display to the player on selection.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the selected custom role from the <see cref="Name"/>.
        /// </summary>
        /// <returns>The custom role or null if it does not exist.</returns>
        public CustomRole GetSelection() => CustomRole.Get(Name);
    }
}