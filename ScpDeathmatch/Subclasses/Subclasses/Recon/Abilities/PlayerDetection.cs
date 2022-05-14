// -----------------------------------------------------------------------
// <copyright file="PlayerDetection.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Recon.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using MEC;

    /// <inheritdoc />
    public class PlayerDetection : PassiveAbility
    {
        private readonly Dictionary<Player, CoroutineHandle> trackingCoroutines = new();
        private readonly Dictionary<Player, HashSet<Player>> inRange = new();

        /// <inheritdoc />
        public override string Name { get; set; } = "Player Detection";

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <summary>
        /// Gets or sets the distance at which players are detected.
        /// </summary>
        [Description("The distance at which players are detected.")]
        public float MinimumDistance { get; set; } = 75f;

        /// <summary>
        /// Gets or sets the alert to send to recons when a player enters their detection zone.
        /// </summary>
        public string AlertEntered { get; set; } = "{0} has entered your detection range";

        /// <summary>
        /// Gets or sets the alert to send to recons when a player leaves their detection zone.
        /// </summary>
        public string AlertLeft { get; set; } = "{0} has left your detection range";

        /// <inheritdoc />
        protected override void AbilityAdded(Player player)
        {
            trackingCoroutines.Add(player, Timing.RunCoroutine(RunTracking(player)));
        }

        /// <inheritdoc />
        protected override void AbilityRemoved(Player player)
        {
            if (trackingCoroutines.TryGetValue(player, out CoroutineHandle coroutineHandle))
            {
                Timing.KillCoroutines(coroutineHandle);
                trackingCoroutines.Remove(player);
            }

            inRange.Remove(player);
        }

        private IEnumerator<float> RunTracking(Player player)
        {
            inRange.Add(player, new HashSet<Player>());

            while (player.IsConnected)
            {
                yield return Timing.WaitForSeconds(2f);
                foreach (Player ply in Player.List)
                {
                    if (ply.SessionVariables.ContainsKey("IsNPC"))
                        continue;

                    bool isInRange = (player.Position - ply.Position).magnitude < MinimumDistance * MinimumDistance;
                    bool previouslyInRange = inRange[player].Contains(ply);
                    if (isInRange && !previouslyInRange)
                    {
                        inRange[player].Add(ply);
                        player.ShowHint(string.Format(AlertEntered, player.DisplayNickname ?? player.Nickname));
                        continue;
                    }

                    if (!isInRange && previouslyInRange)
                    {
                        inRange[player].Remove(ply);
                        player.ShowHint(string.Format(AlertLeft, player.DisplayNickname ?? player.Nickname));
                    }
                }
            }

            trackingCoroutines.Remove(player);
            inRange.Remove(player);
        }
    }
}