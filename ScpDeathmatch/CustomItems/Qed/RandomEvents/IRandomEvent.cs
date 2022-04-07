// -----------------------------------------------------------------------
// <copyright file="IRandomEvent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using Exiled.Events.EventArgs;

    /// <summary>
    /// Defines a contract for random events.
    /// </summary>
    public interface IRandomEvent
    {
        /// <summary>
        /// Gets or sets a value indicating whether the event can be activated.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The action to run when the event is activated.
        /// </summary>
        /// <param name="ev">The <see cref="ExplodingGrenadeEventArgs"/> instance.</param>
        void Action(ExplodingGrenadeEventArgs ev);
    }
}