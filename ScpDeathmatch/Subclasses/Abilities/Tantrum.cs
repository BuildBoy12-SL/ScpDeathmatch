// -----------------------------------------------------------------------
// <copyright file="Tantrum.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Abilities
{
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using Mirror;
    using PlayableScps.ScriptableObjects;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class Tantrum : ActiveAbility
    {
        /// <inheritdoc />
        public override string Name { get; set; } = "Tantrum";

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        [YamlIgnore]
        public override float Duration { get; set; }

        /// <inheritdoc />
        public override float Cooldown { get; set; } = 45f;

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.WalkingOnTantrum += OnWalkingOnTantrum;
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.WalkingOnTantrum -= OnWalkingOnTantrum;
        }

        /// <inheritdoc />
        protected override void AbilityUsed(Player player)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(ScpScriptableObjects.Instance.Scp173Data.TantrumPrefab);
            gameObject.transform.position = player.Position;
            NetworkServer.Spawn(gameObject);
            foreach (TeslaGate teslaGate in TeslaGate.List)
            {
                if (teslaGate.PlayerInIdleRange(player))
                    teslaGate.Base.TantrumsToBeDestroyed.Add(gameObject);
            }
        }

        private void OnWalkingOnTantrum(WalkingOnTantrumEventArgs ev)
        {
            if (Players.Contains(ev.Player))
                ev.IsAllowed = false;
        }
    }
}