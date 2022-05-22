// -----------------------------------------------------------------------
// <copyright file="TranslationsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ScpDeathmatch.Configs.TranslationConfigs;
    using ScpDeathmatch.Configs.TranslationConfigs.Models;

    /// <summary>
    /// Handles miscellaneous translations.
    /// </summary>
    public class TranslationsConfig
    {
        private Dictionary<Type, PropertyInfo> cachedProperties;

        /// <summary>
        /// Gets or sets the translations for items.
        /// </summary>
        public ItemTranslations ItemTranslations { get; set; } = new();

        /// <summary>
        /// Gets the translation for the specified enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum to translate.</typeparam>
        /// <param name="enum">The enum to translate.</param>
        /// <returns>The translation or <see cref="Enum.ToString()"/> if one is not found.</returns>
        public string Get<T>(T @enum)
            where T : Enum
        {
            cachedProperties ??= GenerateCache();
            if (cachedProperties.TryGetValue(typeof(T), out PropertyInfo property) &&
                property.GetValue(this) is ITranslation<T> translation &&
                translation.Translations.TryGetValue(@enum, out string translationString))
                return translationString;

            return @enum.ToString();
        }

        private Dictionary<Type, PropertyInfo> GenerateCache()
        {
            Dictionary<Type, PropertyInfo> cache = new Dictionary<Type, PropertyInfo>();
            foreach (PropertyInfo property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.PropertyType.GetInterface(typeof(ITranslation<>).Name) is Type type)
                    cache.Add(type.GetGenericArguments()[0], property);
            }

            return cache;
        }
    }
}