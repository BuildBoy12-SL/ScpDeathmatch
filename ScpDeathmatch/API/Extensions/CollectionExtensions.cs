// -----------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.API.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Various extension methods for the <see cref="IList{T}"/> interface.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Picks a random element from a list, or <see langword="default"/> if the list is empty.
        /// </summary>
        /// <param name="list">The list to pick an element from.</param>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <returns>A random element from the list.</returns>
        /// <exception cref="ArgumentNullException">The provided list cannot be null.</exception>
        public static T Random<T>(this IList<T> list)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            return list.IsEmpty() ? default : list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Picks a random element from a list, or <see langword="default"/> if the list is empty.
        /// </summary>
        /// <param name="valueCollection">The collection to pick an element from.</param>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <returns>A random element from the list.</returns>
        /// <exception cref="ArgumentNullException">The provided list cannot be null.</exception>
        public static TValue Random<TKey, TValue>(this Dictionary<TKey, TValue>.ValueCollection valueCollection)
        {
            if (valueCollection is null)
                throw new ArgumentNullException(nameof(valueCollection));

            return valueCollection.ElementAt(UnityEngine.Random.Range(0, valueCollection.Count));
        }

        /// <summary>
        /// Picks a random element from a list, or <see langword="default"/> if the list is empty.
        /// </summary>
        /// <param name="enumerable">The enumerable to pick an element from.</param>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <returns>A random element from the list.</returns>
        /// <exception cref="ArgumentNullException">The provided list cannot be null.</exception>
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));

            return Random(enumerable.ToList());
        }
    }
}