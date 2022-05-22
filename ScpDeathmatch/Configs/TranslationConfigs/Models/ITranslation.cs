// -----------------------------------------------------------------------
// <copyright file="ITranslation.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs.TranslationConfigs.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the contract for translating enums.
    /// </summary>
    /// <typeparam name="TEnum">The enum to translate.</typeparam>
    public interface ITranslation<TEnum>
        where TEnum : Enum
    {
        /// <summary>
        /// Gets or sets the translations.
        /// </summary>
        public Dictionary<TEnum, string> Translations { get; set; }
    }
}