// -----------------------------------------------------------------------
// <copyright file="DecontaminationManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Decontamination
{
    using System.Collections.Generic;
    using LightContainmentZoneDecontamination;
    using MEC;
    using ScpDeathmatch.Decontamination.Models;

    /// <summary>
    /// Handles the decontamination sequence.
    /// </summary>
    public class DecontaminationManager
    {
        private readonly Plugin plugin;
        private CoroutineHandle coroutineHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecontaminationManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public DecontaminationManager(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        private void OnWaitingForPlayers()
        {
            if (plugin.Config.Decontamination.IsEnabled)
            {
                DecontaminationController.Singleton.NetworkRoundStartTime = -1.0;
                DecontaminationController.Singleton._stopUpdating = true;
            }

            if (coroutineHandle.IsRunning)
               Timing.KillCoroutines(coroutineHandle);
        }

        private void OnRoundStarted()
        {
            if (plugin.Config.Decontamination.IsEnabled)
                coroutineHandle = Timing.RunCoroutine(RunDecontamination());
        }

        private IEnumerator<float> RunDecontamination()
        {
            foreach (DecontaminationPhase decontaminationPhase in plugin.Config.Decontamination.Phases)
            {
                yield return Timing.WaitForSeconds(decontaminationPhase.TriggerTime);
                decontaminationPhase.Run();
            }
        }
    }
}