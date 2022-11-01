// -----------------------------------------------------------------------
// <copyright file="Hint.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using AdvancedHints;
    using AdvancedHints.Enums;
    using Exiled.API.Features;

    /// <summary>
    /// A wrapper to save hints to be shown to a player.
    /// </summary>
    public class Hint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hint"/> class.
        /// </summary>
        public Hint()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hint"/> class.
        /// </summary>
        /// <param name="message"><inheritdoc cref="Message"/></param>
        /// <param name="duration"><inheritdoc cref="Duration"/></param>
        /// <param name="show"><inheritdoc cref="Show"/></param>
        /// <param name="displayLocation"><inheritdoc cref="DisplayLocation"/></param>
        public Hint(string message, float duration, bool show = true, DisplayLocation displayLocation = DisplayLocation.MiddleBottom)
        {
            Message = message;
            Duration = duration;
            Show = show;
            DisplayLocation = displayLocation;
        }

        /// <summary>
        /// Gets or sets the hint to be displayed.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the duration the hint should display.
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the hint should be displayed.
        /// </summary>
        public bool Show { get; set; }

        /// <summary>
        /// Gets or sets the location to display the hint.
        /// </summary>
        public DisplayLocation DisplayLocation { get; set; }

        /// <summary>
        /// Displays the hint to the specified player.
        /// </summary>
        /// <param name="player">The player to display the hint to.</param>
        public void Display(Player player)
        {
            if (Show)
                player.ShowHint(Message, Duration);
        }

        /// <summary>
        /// Displays the hint to the specified player.
        /// </summary>
        /// <param name="player">The player to display the hint to.</param>
        /// <param name="args">The arguments to <see cref="string.Format(string,object[])">format</see> the <see cref="Message"/>.</param>
        public void DisplayFormatted(Player player, params object[] args)
        {
            if (Show && args is not null && args.Length > 0)
                player.ShowHint(string.Format(Message, args), Duration);
        }

        /// <summary>
        /// Displays the hint to the specified player.
        /// </summary>
        /// <param name="player">The player to display the hint to.</param>
        /// <param name="overrideQueue">Whether this hint should take priority over all other active hints.</param>
        public void DisplayManaged(Player player, bool overrideQueue = true)
        {
            if (Show)
                player.ShowManagedHint(Message, Duration, overrideQueue, DisplayLocation);
        }
    }
}