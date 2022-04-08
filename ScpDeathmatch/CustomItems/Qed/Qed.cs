// -----------------------------------------------------------------------
// <copyright file="Qed.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.CustomItems.Qed.RandomEvents;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class Qed : CustomGrenade
    {
        /// <inheritdoc />
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.GrenadeFlash;

        /// <inheritdoc />
        public override uint Id { get; set; } = 125;

        /// <inheritdoc />
        public override string Name { get; set; } = "QED";

        /// <inheritdoc />
        public override string Description { get; set; } = "A device that will initiate a random event on detonation";

        /// <inheritdoc />
        public override float Weight { get; set; } = 0f;

        /// <inheritdoc />
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties
        {
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint { Chance = 100f, Location = SpawnLocation.InsideGr18, },
                new DynamicSpawnPoint { Chance = 100f, Location = SpawnLocation.Inside049Armory, },
                new DynamicSpawnPoint { Chance = 100f, Location = SpawnLocation.InsideGateA, },
            },
        };

        /// <inheritdoc />
        public override bool ExplodeOnCollision { get; set; } = false;

        /// <inheritdoc />
        public override float FuseTime { get; set; } = 3f;

        /// <summary>
        /// Gets or sets a value indicating whether debug logs will be shown.
        /// </summary>
        public bool ShowDebug { get; set; } = false;

        /// <summary>
        /// Gets or sets the events to pick from.
        /// </summary>
        public RandomEventsConfig RandomEvents { get; set; } = new RandomEventsConfig();

        /// <inheritdoc />
        public override void Init()
        {
            RandomEvents?.Reload();
            base.Init();
        }

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.ReloadedConfigs += OnReloadedConfigs;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.ReloadedConfigs -= OnReloadedConfigs;
            base.UnsubscribeEvents();
        }

        /// <inheritdoc />
        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            ev.TargetsToAffect.Clear();
            IRandomEvent randomEvent = RandomEvents?.FindRandom();
            if (randomEvent == null)
                return;

            Log.Debug("Executing random event: " + randomEvent.Name, ShowDebug);
            randomEvent.OnExploding(ev);
        }

        private void OnReloadedConfigs() => RandomEvents?.Reload();
    }
}