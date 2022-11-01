// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses
{
    using Exiled.API.Features;

    /// <summary>
    /// Miscellaneous extensions related to subclasses.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Checks whether the player is a subclass of the specified type.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <typeparam name="T">The subclass to check against.</typeparam>
        /// <returns>Whether the player has the specified subclass.</returns>
        public static bool IsSubclass<T>(this Player player)
            where T : Subclass
        {
            Subclass subclass = Subclass.Get(typeof(T));
            return subclass != null && subclass.Check(player);
        }

        /// <summary>
        /// Checks whether the player is a subclass of the specified type.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <param name="subclass">The subclass instance.</param>
        /// <typeparam name="T">The subclass to check against.</typeparam>
        /// <returns>Whether the player has the specified subclass.</returns>
        public static bool IsSubclass<T>(this Player player, T subclass)
            where T : Subclass
        {
            return subclass.Check(player);
        }
    }
}