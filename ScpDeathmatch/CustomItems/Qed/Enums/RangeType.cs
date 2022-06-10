// -----------------------------------------------------------------------
// <copyright file="RangeType.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.Enums
{
    /// <summary>
    /// Defines the distance types that events can have.
    /// </summary>
    public enum RangeType
    {
        /// <summary>
        /// Indicates the event can be fired when close to the player.
        /// </summary>
        Close,

        /// <summary>
        /// Indicates the event can be fired when between <see cref="Close"/> and <see cref="Far"/>.
        /// </summary>
        Neutral,

        /// <summary>
        /// Indicates the event can be fired when far away from the player.
        /// </summary>
        Far,
    }
}