// -----------------------------------------------------------------------
// <copyright file="DecontaminationManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System;
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Interactables.Interobjects.DoorUtils;
    using LightContainmentZoneDecontamination;
    using MEC;
    using Mirror;
    using ScpDeathmatch.Models;
    using Subtitles;
    using UnityEngine;

    /// <summary>
    /// Handles the decontamination sequence.
    /// </summary>
    public class DecontaminationManager
    {
        private readonly DecontaminationController decontaminationController;
        private readonly List<DecontaminationPhase> decontaminationPhases = new List<DecontaminationPhase>
        {
        };

        private CoroutineHandle coroutineHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecontaminationManager"/> class.
        /// </summary>
        public DecontaminationManager()
        {
            decontaminationController = UnityEngine.Object.FindObjectOfType<DecontaminationController>();
        }

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
            if (coroutineHandle.IsRunning)
                Timing.KillCoroutines(coroutineHandle);
        }

        private void OnRoundStarted()
        {
            coroutineHandle = Timing.RunCoroutine(RunDecontamination());
        }

        private IEnumerator<float> RunDecontamination()
        {
            int nextPhase = 0;
            yield return Timing.WaitForSeconds(10f);

            // 5 Minutes
            HandlePhase(nextPhase++);
            yield return Timing.WaitForSeconds(240f);

            // 1 Minute
            HandlePhase(nextPhase++);
            yield return Timing.WaitForSeconds(30f);

            // 30 Seconds
            HandlePhase(nextPhase++);
            yield return Timing.WaitForSeconds(30f);

            HandlePhase(nextPhase);
        }

        private void HandlePhase(int index)
        {
            try
            {
                DecontaminationController.DecontaminationPhase phase = decontaminationController.DecontaminationPhases[index];
                Log.Warn(1);
                PlayAnnouncement(phase);
                Log.Warn(2);
                PlaySubtitles(index, phase.Function);
                Log.Warn(3);

                if (phase.Function == DecontaminationController.DecontaminationPhase.PhaseFunction.Final)
                    decontaminationController.FinishDecontamination();

                Log.Warn(4);
                if (NetworkServer.active && phase.Function == DecontaminationController.DecontaminationPhase.PhaseFunction.OpenCheckpoints)
                    DoorEventOpenerExtension.TriggerAction(DoorEventOpenerExtension.OpenerEventType.DeconEvac);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        private void PlayAnnouncement(DecontaminationController.DecontaminationPhase phase)
        {
            decontaminationController.AnnouncementAudioSource.PlayOneShot(phase.AnnouncementLine);
        }

        private void PlaySubtitles(int index, DecontaminationController.DecontaminationPhase.PhaseFunction function)
        {
            foreach (ReferenceHub allHub in ReferenceHub.GetAllHubs().Values)
            {
                ReferenceHub cameraHub = allHub.spectatorManager.GetCameraHub();
                if (function == DecontaminationController.DecontaminationPhase.PhaseFunction.Final || function == DecontaminationController.DecontaminationPhase.PhaseFunction.GloballyAudible || Mathf.Abs(cameraHub.playerMovementSync.GetRealPosition().y) < 200.0)
                    allHub.networkIdentity.connectionToClient.Send(new SubtitleMessage(Subtitles(index).ToArray()));
            }
        }

        private List<SubtitlePart> Subtitles(int index)
        {
            List<SubtitlePart> subtitlePartList = new List<SubtitlePart>(1);

            switch (index)
            {
                case 0:
                    subtitlePartList.Add(new SubtitlePart(SubtitleType.DecontaminationMinutes, new[] { "5" }));
                    break;
                case 1:
                    subtitlePartList.Add(new SubtitlePart(SubtitleType.Decontamination1Minute, null));
                    break;
                case 2:
                    subtitlePartList.Add(new SubtitlePart(SubtitleType.DecontaminationCountdown, null));
                    break;
                case 3:
                    subtitlePartList.Add(new SubtitlePart(SubtitleType.DecontaminationLockdown, null));
                    break;
            }

            return subtitlePartList;
        }
    }
}