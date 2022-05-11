// -----------------------------------------------------------------------
// <copyright file="RespawnManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Collections.Generic;
    using MEC;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Manages an underlying collection of players that are considered to be respawning.
    /// </summary>
    public static class RespawnManager
    {
        private static readonly List<Respawner> ToRespawn = new();

        /// <summary>
        /// Gets the amount of players that are considered to be respawning.
        /// </summary>
        public static int Count => ToRespawn.Count;

        /// <summary>
        /// Adds a spawner to the respawn collection, verifying them after 0.5 seconds.
        /// </summary>
        /// <param name="spawner">The spawner to add.</param>
        public static void Add(Respawner spawner)
        {
            ToRespawn.Add(spawner);
            Timing.RunCoroutine(RunChecks(spawner));
        }

        private static IEnumerator<float> RunChecks(Respawner spawner)
        {
            yield return Timing.WaitForSeconds(0.5f);
            if (spawner.VerifyCondition())
            {
                spawner.Respawn();
            }
            else
            {
                spawner.Fail();
                spawner.Dispose();
            }

            yield return Timing.WaitForSeconds(1f);
            ToRespawn.Remove(spawner);
        }
    }
}