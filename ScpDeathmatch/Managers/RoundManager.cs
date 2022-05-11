// -----------------------------------------------------------------------
// <copyright file="RoundManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Manages deciding when the round should ends.
    /// </summary>
    public class RoundManager : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public RoundManager(Plugin plugin)
            : base(plugin)
        {
        }

        private int AliveCount
        {
            get
            {
                int aliveCount = 0;
                foreach (Player player in Player.List)
                {
                    if (player.SessionVariables.ContainsKey("IsNPC"))
                        continue;

                    if (Plugin.Config.Subclasses.Insurgent.Check(player) &&
                        !Plugin.Config.Subclasses.Insurgent.Count079Alive &&
                        player.Role.Type == RoleType.Scp079)
                        continue;

                    if (player.IsAlive)
                        aliveCount++;
                }

                return aliveCount;
            }
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Server.EndingRound += OnEndingRound;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Server.EndingRound -= OnEndingRound;
        }

        private void OnEndingRound(EndingRoundEventArgs ev)
        {
            ev.IsRoundEnded = AliveCount + RespawnManager.Count <= 1;
            bool spawnedMore = false;
            if (ev.IsRoundEnded)
            {
                foreach (Player player in Player.List)
                {
                    if (!Plugin.Config.Subclasses.Insurgent.Check(player) || player.Role.Type != RoleType.Scp079)
                        continue;

                    player.Role.Type = RoleType.Scientist;
                    spawnedMore = true;
                }
            }

            if (spawnedMore)
                ev.IsRoundEnded = false;

            ev.IsAllowed = true;
        }
    }
}