// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch
{
    using System;
    using Exiled.API.Features;
    using HarmonyLib;
    using RemoteAdmin;
    using ScpDeathmatch.Decontamination;
    using ScpDeathmatch.EventHandlers;
    using ScpDeathmatch.KillRewards;
    using ScpDeathmatch.Managers;

    /// <inheritdoc />
    public class Plugin : Plugin<Config>
    {
        private Harmony harmony;

        private ArmoryPitManager armoryPitManager;
        private DecontaminationManager decontaminationManager;
        private DisarmingLivesManager disarmingLivesManager;
        private MicroHidHealing microHidHealing;
        private RewardManager rewardManager;
        private RoundStatsManager roundStatsManager;
        private TimedCommandHandler timedCommandHandler;

        private ServerEvents serverEvents;

        /// <summary>
        /// Gets an instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <summary>
        /// Gets an instance of the <see cref="ZoneAnnouncer"/> class.
        /// </summary>
        public ZoneAnnouncer ZoneAnnouncer { get; private set; }

        /// <summary>
        /// Gets an instance of the <see cref="Managers.RespawnManager"/> class.
        /// </summary>
        public RespawnManager RespawnManager { get; private set; }

        /// <inheritdoc/>
        public override string Author => "Build";

        /// <inheritdoc/>
        public override string Name => "ScpDeathmatch";

        /// <inheritdoc/>
        public override string Prefix => "ScpDeathmatch";

        /// <inheritdoc/>
        public override Version Version { get; } = new Version(1, 0, 0);

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;

            harmony = new Harmony($"deathMath.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            armoryPitManager = new ArmoryPitManager(this);
            armoryPitManager.Subscribe();

            decontaminationManager = new DecontaminationManager(this);
            decontaminationManager.Subscribe();

            disarmingLivesManager = new DisarmingLivesManager(this);
            disarmingLivesManager.Subscribe();

            microHidHealing = new MicroHidHealing(this);
            microHidHealing.Subscribe();

            RespawnManager = new RespawnManager();

            rewardManager = new RewardManager(this);
            rewardManager.Subscribe();

            roundStatsManager = new RoundStatsManager(this);
            roundStatsManager.Subscribe();

            timedCommandHandler = new TimedCommandHandler(this);
            timedCommandHandler.Subscribe();

            ZoneAnnouncer = new ZoneAnnouncer(this);
            ZoneAnnouncer.Subscribe();

            serverEvents = new ServerEvents(this);
            serverEvents.Subscribe();

            Config.CustomItems.Register();
            Config.CustomRoles.Register();

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Config.CustomItems.Unregister();
            Config.CustomRoles.Unregister();

            serverEvents?.Unsubscribe();
            serverEvents = null;

            ZoneAnnouncer?.Unsubscribe();
            ZoneAnnouncer = null;

            timedCommandHandler?.Unsubscribe();
            timedCommandHandler = null;

            roundStatsManager?.Unsubscribe();
            roundStatsManager = null;

            rewardManager?.Unsubscribe();
            rewardManager = null;

            RespawnManager = null;

            microHidHealing?.Unsubscribe();
            microHidHealing = null;

            disarmingLivesManager?.Unsubscribe();
            disarmingLivesManager = null;

            decontaminationManager?.Unsubscribe();
            decontaminationManager = null;

            armoryPitManager?.Unsubscribe();
            armoryPitManager = null;

            harmony?.UnpatchAll(harmony.Id);
            harmony = null;

            Instance = null;

            base.OnDisabled();
        }

        /// <inheritdoc />
        public override void OnRegisteringCommands()
        {
            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(Config.ZoneAnnouncer.ForceAnnouncerCommand);
        }

        /// <inheritdoc />
        public override void OnUnregisteringCommands()
        {
            CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(Config.ZoneAnnouncer.ForceAnnouncerCommand);
        }
    }
}