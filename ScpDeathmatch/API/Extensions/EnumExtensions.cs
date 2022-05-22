// -----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.API.Extensions
{
    using System;

    /// <summary>
    /// Miscellaneous extensions for the <see cref="Enum"/> class.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the translation for the specified enum.
        /// </summary>
        /// <param name="enum">The enum to translate.</param>
        /// <returns>The translation or <see cref="Enum.ToString()"/> if one is not found.</returns>
        public static string Translation(this Enum @enum) => Plugin.Instance.Config.Translations.Get(@enum);
    }
}