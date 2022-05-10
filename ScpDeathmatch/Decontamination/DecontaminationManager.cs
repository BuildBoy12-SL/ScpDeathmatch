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
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles the decontamination sequence.
    /// </summary>
    public class DecontaminationManager : Subscribable
    {
        private CoroutineHandle coroutineHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecontaminationManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public DecontaminationManager(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        }

        /// <inheritdoc/>
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        }

        private void OnWaitingForPlayers()
        {
            if (Plugin.Config.Decontamination.IsEnabled)
            {
                DecontaminationController.Singleton.NetworkRoundStartTime = -1.0;
                DecontaminationController.Singleton._stopUpdating = true;
            }

            if (coroutineHandle.IsRunning)
               Timing.KillCoroutines(coroutineHandle);
        }

        private void OnRoundStarted()
        {
            if (coroutineHandle.IsRunning)
                Timing.KillCoroutines(coroutineHandle);

            if (Plugin.Config.Decontamination.IsEnabled)
                coroutineHandle = Timing.RunCoroutine(RunDecontamination());
        }

        private IEnumerator<float> RunDecontamination()
        {
            foreach (DecontaminationPhase decontaminationPhase in Plugin.Config.Decontamination.Phases)
            {
                yield return Timing.WaitForSeconds(decontaminationPhase.TriggerTime);
                decontaminationPhase.Run();
            }
        }
    }
}