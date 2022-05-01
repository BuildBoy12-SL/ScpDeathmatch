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

    /// <summary>
    /// Manages deciding when the round should ends.
    /// </summary>
    public class RoundManager
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public RoundManager(Plugin plugin) => this.plugin = plugin;

        private int AliveCount
        {
            get
            {
                int aliveCount = 0;
                foreach (Player player in Player.List)
                {
                    if (player.SessionVariables.ContainsKey("IsNPC"))
                        continue;

                    if (plugin.Config.Subclasses.Insurgent.Check(player) &&
                        !plugin.Config.Subclasses.Insurgent.Count079Alive &&
                        player.Role.Type == RoleType.Scp079)
                        continue;

                    if (player.IsAlive)
                        aliveCount++;
                }

                return aliveCount;
            }
        }

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Server.EndingRound += OnEndingRound;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Server.EndingRound -= OnEndingRound;
        }

        private void OnEndingRound(EndingRoundEventArgs ev)
        {
            ev.IsRoundEnded = AliveCount + plugin.RespawnManager.Count <= 1;
            ev.IsAllowed = true;
        }
    }
}