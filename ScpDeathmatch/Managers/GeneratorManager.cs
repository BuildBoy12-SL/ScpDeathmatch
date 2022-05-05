// -----------------------------------------------------------------------
// <copyright file="GeneratorManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Manages Scp079 generators.
    /// </summary>
    public class GeneratorManager : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public GeneratorManager(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Map.GeneratorActivated += OnGeneratorActivated;
            Exiled.Events.Handlers.Player.UnlockingGenerator += OnUnlockingGenerator;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Map.GeneratorActivated -= OnGeneratorActivated;
            Exiled.Events.Handlers.Player.UnlockingGenerator -= OnUnlockingGenerator;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        private void OnGeneratorActivated(GeneratorActivatedEventArgs ev)
        {
            int activated = Generator.Get(GeneratorState.Engaged).Count() + 1;
            int total = Generator.List.Count();
            Cassie.Message(string.Format(Plugin.Config.Generators.EngagedAnnouncement, activated, total), isNoisy: false);
            if (activated == total)
                Cassie.Message(Plugin.Config.Generators.AllEngagedAnnouncement, isNoisy: false);
        }

        private void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (Round.ElapsedTime.TotalSeconds < Plugin.Config.Generators.UnlockDelay)
                ev.IsAllowed = false;
        }

        private void OnRoundStarted()
        {
            foreach (Generator generator in Generator.List)
                generator.ActivationTime = Plugin.Config.Generators.DefaultTime;
        }
    }
}