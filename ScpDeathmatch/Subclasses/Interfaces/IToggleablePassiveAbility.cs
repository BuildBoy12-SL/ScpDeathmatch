// -----------------------------------------------------------------------
// <copyright file="IToggleablePassiveAbility.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Interfaces
{
    using Exiled.API.Features;

    /// <summary>
    /// Defines the contract for a toggleable passive ability.
    /// </summary>
    public interface IToggleablePassiveAbility
    {
        /// <summary>
        /// Toggles the ability.
        /// </summary>
        /// <param name="player">The player toggling the ability.</param>
        /// <param name="response">The response to send to the player.</param>
        /// <returns>Whether the ability was successfully toggled.</returns>
        bool Toggle(Player player, out string response);
    }
}