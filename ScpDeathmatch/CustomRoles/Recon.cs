// -----------------------------------------------------------------------
// <copyright file="Recon.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomRoles
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using MEC;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class Recon : Subclass
    {
        private readonly Dictionary<Player, ZoneType> previousZones = new();
        private CoroutineHandle coroutineHandle;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Recon);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        public override string Badge { get; set; } = nameof(Recon);

        /// <inheritdoc />
        public override string BadgeColor { get; set; } = "mint";

        /// <inheritdoc />
        public override List<string> Inventory { get; set; } = new()
        {
            "Goggle Toggle",
        };

        /// <summary>
        /// Gets or sets the alert to send to recons when a player enters their zone.
        /// </summary>
        public string AlertEntered { get; set; } = "{0} has entered your current zone";

        /// <summary>
        /// Gets or sets the alert to send to recons when a player leaves their zone.
        /// </summary>
        public string AlertLeft { get; set; } = "{0} has left your current zone";

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; } = new();

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            base.UnsubscribeEvents();
        }

        private void OnRoundStarted()
        {
            if (coroutineHandle.IsRunning)
                Timing.KillCoroutines(coroutineHandle);

            coroutineHandle = Timing.RunCoroutine(RunZoneTracking());
        }

        private IEnumerator<float> RunZoneTracking()
        {
            yield return Timing.WaitForSeconds(3f);
            foreach (Player player in Player.List)
                previousZones.Add(player, player.Zone);

            while (Round.IsStarted)
            {
                foreach (Player player in Player.List)
                {
                    if (previousZones.TryGetValue(player, out ZoneType zoneType) && zoneType != player.Zone)
                        Alert(player, zoneType);

                    previousZones[player] = player.Zone;
                }

                yield return Timing.WaitForSeconds(2f);
            }

            previousZones.Clear();
        }

        private void Alert(Player player, ZoneType previousZone)
        {
            foreach (Player recon in TrackedPlayers)
            {
                if (player == recon)
                    continue;

                if (recon.Zone == previousZone)
                {
                    recon.ShowHint(string.Format(AlertLeft, player.DisplayNickname ?? player.Nickname));
                    continue;
                }

                if (recon.Zone == player.Zone)
                {
                    recon.ShowHint(string.Format(AlertEntered, player.DisplayNickname ?? player.Nickname));
                }
            }
        }
    }
}