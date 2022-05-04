// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch
{
    using System;
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using HarmonyLib;
    using RemoteAdmin;
    using ScpDeathmatch.Commands;
    using ScpDeathmatch.Managers;
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Patches.Manual;
    using ScpDeathmatch.Stats;

    /// <inheritdoc />
    public class Plugin : Plugin<Config>
    {
        private readonly List<Subscribable> subscribed = new();
        private Harmony harmony;

        /// <summary>
        /// Gets an instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <summary>
        /// Gets an instance of the <see cref="Managers.RespawnManager"/> class.
        /// </summary>
        public RespawnManager RespawnManager { get; private set; }

        /// <summary>
        /// Gets an instance of the <see cref="Stats.StatDatabase"/> class.
        /// </summary>
        public StatDatabase StatDatabase { get; private set; }

        /// <summary>
        /// Gets an instance of the <see cref="ZoneAnnouncer"/> class.
        /// </summary>
        public ZoneAnnouncer ZoneAnnouncer { get; private set; }

        /// <inheritdoc/>
        public override string Author => "Build";

        /// <inheritdoc/>
        public override string Name => "ScpDeathmatch";

        /// <inheritdoc/>
        public override string Prefix => "ScpDeathmatch";

        /// <inheritdoc />
        public override PluginPriority Priority => PluginPriority.Lowest;

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new(5, 2, 1);

        /// <inheritdoc/>
        public override Version Version { get; } = new(1, 0, 0);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;

            Config.Reload();

            harmony = new Harmony($"deathMatch.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();
            PatchManual();

            RespawnManager = new RespawnManager();

            StatDatabase = new StatDatabase(this);
            StatDatabase.Open();

            ZoneAnnouncer = new ZoneAnnouncer(this);
            ZoneAnnouncer.Subscribe();

            Subscribe();

            Config.CustomItems.Register();
            Config.Subclasses.Register();

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Config.CustomItems.Unregister();
            Config.Subclasses.Unregister();

            Unsubscribe();

            ZoneAnnouncer?.Unsubscribe();
            ZoneAnnouncer = null;

            StatDatabase?.Close();
            StatDatabase = null;

            RespawnManager = null;

            harmony?.UnpatchAll(harmony.Id);
            harmony = null;

            Instance = null;

            base.OnDisabled();
        }

        /// <inheritdoc />
        public override void OnRegisteringCommands()
        {
            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(Config.StatsDatabase.ClearStatsCommand);
            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(Config.ZoneAnnouncer.ForceAnnouncerCommand);
            Config.MiscCommands.Register();

            Config.ClientCommands.Register();
        }

        /// <inheritdoc />
        public override void OnUnregisteringCommands()
        {
            CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(Config.StatsDatabase.ClearStatsCommand);
            CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(Config.ZoneAnnouncer.ForceAnnouncerCommand);
            Config.MiscCommands.Unregister();

            Config.ClientCommands.Unregister();
        }

        private void PatchManual()
        {
            foreach (Type type in Assembly.GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IManualPatch)))
                    ((IManualPatch)Activator.CreateInstance(type)).Patch(harmony);
            }
        }

        private void Subscribe()
        {
            foreach (Type type in Assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(Subscribable)))
                {
                    Subscribable subscribable = (Subscribable)Activator.CreateInstance(type, args: this);
                    subscribable.Subscribe();
                    subscribed.Add(subscribable);
                }
            }
        }

        private void Unsubscribe()
        {
            foreach (Subscribable subscribable in subscribed)
                subscribable.Unsubscribe();

            subscribed.Clear();
        }
    }
}