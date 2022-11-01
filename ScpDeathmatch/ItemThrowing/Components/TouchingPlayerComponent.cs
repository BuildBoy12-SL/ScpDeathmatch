// -----------------------------------------------------------------------
// <copyright file="TouchingPlayerComponent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.ItemThrowing.Components
{
    using System;
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using MEC;
    using ScpDeathmatch.ItemThrowing.Models;
    using UnityEngine;

    /// <inheritdoc />
    public class TouchingPlayerComponent : MonoBehaviour
    {
        private static bool Debug => Plugin.Instance.Config.ItemThrowing.Debug;

        /// <summary>
        /// Initializes the component.
        /// </summary>
        /// <param name="owner">The previous owner of the pickup.</param>
        /// <param name="pickup">The thrown pickup.</param>
        public void Init(Player owner, Pickup pickup)
        {
            if (Plugin.Instance.Config.ItemThrowing.Settings.TryGetValue(pickup.Type, out ThrowSettings throwSettings))
                Timing.RunCoroutine(RunCollisionCheck(owner, pickup, throwSettings));
        }

        private IEnumerator<float> RunCollisionCheck(Player owner, Pickup pickup, ThrowSettings throwSettings)
        {
            List<int> hits = new();
            int internalChecks = 0;
            while (true)
            {
                try
                {
                    if (!pickup.Base.Rb || pickup.Base.Rb.velocity == null)
                    {
                        Log.Debug("Pickup has no rigidbody or velocity is null.", Debug);
                        break;
                    }

                    if (pickup.Base.Rb.velocity.y == 0 && internalChecks > 5)
                    {
                        Log.Debug("Breaking loop, vertical velocity hit zero.", Debug);
                        break;
                    }

                    internalChecks++;
                    foreach (var player in Player.List)
                    {
                        if (player == null ||
                            string.IsNullOrEmpty(player.UserId) ||
                            player == owner ||
                            hits.Contains(player.Id) ||
                            player.SessionVariables.ContainsKey("IsNPC") ||
                            player.SessionVariables.ContainsKey("IsGhostSpectator") ||
                            Vector3.Distance(pickup.Position, player.Position) > 2f)
                        {
                            continue;
                        }

                        hits.Add(player.Id);
                        Log.Debug($"Hit player {player.Nickname} with {pickup.Type}", Debug);
                        ItemSettings itemSettings = player.Role.Side == owner.Role.Side
                            ? throwSettings.FriendlySettings
                            : throwSettings.EnemySettings;

                        if (itemSettings == null)
                        {
                            Log.Debug("Settings for this item scenario are null", Debug);
                            continue;
                        }

                        Log.Debug("Applying settings to target", Debug);
                        itemSettings.ApplyTo(player, owner);
                        if (itemSettings.ShouldDelete)
                        {
                            pickup.Destroy();
                            Destroy(this);
                            Log.Debug("Should delete is true, destroying pickup and component", Debug);
                            yield break;
                        }

                        if (!itemSettings.HitMultiple)
                        {
                            Destroy(this);
                            Log.Debug("Hit multiple is false, destroying component", Debug);
                            yield break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error while checking for item collision:\n" + e);
                }

                yield return Timing.WaitForOneFrame;
            }

            Destroy(this);
            Log.Debug("Destroying component, checks completed.", Debug);
        }

        /*
        private void OnCollisionEnter(Collision collision)
        {
            if (!isInitialized)
                return;

            if (pickup.Base.Rb.velocity.y == 0)
                return;

            Log.Debug("Collision detected", Debug);
            if (!collision.gameObject.TryGetComponent(out ReferenceHub targetHub)
                || Player.Get(targetHub) is not Player target
                || target == owner
                || hits.Contains(target))
                return;

            Log.Debug("Collision with player detected", Debug);
            hits.Add(target);
            ItemSettings itemSettings = target.Role.Side == owner.Role.Side
                ? throwSettings.FriendlySettings
                : throwSettings.EnemySettings;

            if (itemSettings == null)
            {
                Log.Debug("Settings for this item scenario are null", Debug);
                return;
            }

            Log.Debug("Applying settings to target", Debug);
            itemSettings.ApplyTo(target);
            if (itemSettings.ShouldDelete)
            {
                pickup.Destroy();
                Destroy(this);
                Log.Debug("Should delete is true, destroying pickup and component", Debug);
                return;
            }

            if (!itemSettings.HitMultiple)
            {
                Log.Debug("Hit multiple is false, destroying component", Debug);
                Destroy(this);
            }
        }*/
    }
}