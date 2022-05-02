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
    using Exiled.API.Features;
    using UnityEngine;

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

        /// <summary>
        /// Finds an element in an <see cref="IEnumerable{T}"/> that satisfies the distance and predicate if provided.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to return the element from.</param>
        /// <param name="position">The position to compare each element with.</param>
        /// <param name="maxDistance">The maximum distance the object should be from the specified position.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="T">The type of the elements in enumerable.</typeparam>
        /// <returns>The closest object or the <see langword="default"/> value if no object meets the specified parameters.</returns>
        /// <exception cref="ArgumentNullException">The provided enumerable cannot be null.</exception>
        public static T Closest<T>(this IEnumerable<T> enumerable, Vector3 position, float maxDistance = float.MaxValue, Func<T, bool> predicate = null)
            where T : MonoBehaviour
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));

            float closest = maxDistance;
            T closestObject = default;
            foreach (T obj in enumerable)
            {
                if (predicate != null && !predicate(obj))
                    continue;

                float distance = (obj.transform.position - position).sqrMagnitude;
                if (distance < closest)
                {
                    closest = distance;
                    closestObject = obj;
                }
            }

            return closestObject;
        }

        /// <inheritdoc cref="Closest{T}"/>
        public static Player Closest(this IEnumerable<Player> enumerable, Vector3 position, float maxDistance = float.MaxValue, Func<Player, bool> predicate = null)
        {
            float closest = maxDistance;
            Player closestObject = null;
            foreach (Player player in enumerable)
            {
                if (predicate != null && !predicate(player))
                    continue;

                float distance = (player.Position - position).sqrMagnitude;
                if (distance < closest)
                {
                    closest = distance;
                    closestObject = player;
                }
            }

            return closestObject;
        }

        /// <inheritdoc cref="Closest{T}"/>
        public static Door Closest(this IEnumerable<Door> enumerable, Vector3 position, float maxDistance = float.MaxValue, Func<Door, bool> predicate = null)
        {
            float closest = maxDistance;
            Door closestObject = null;
            foreach (Door door in enumerable)
            {
                if (predicate != null && !predicate(door))
                    continue;

                float distance = (door.Position - position).sqrMagnitude;
                if (distance < closest)
                {
                    closest = distance;
                    closestObject = door;
                }
            }

            return closestObject;
        }
    }
}