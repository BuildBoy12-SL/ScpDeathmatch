// -----------------------------------------------------------------------
// <copyright file="DecontaminationPhase.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Decontamination.Models
{
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Interactables.Interobjects.DoorUtils;

    /// <summary>
    /// Represents a phase in the decontamination sequence.
    /// </summary>
    public class DecontaminationPhase
    {
        private float triggerTime;

        /// <summary>
        /// Gets or sets the broadcast message to display.
        /// </summary>
        public Broadcast Broadcast { get; set; }

        /// <summary>
        /// Gets or sets the cassie message to play.
        /// </summary>
        public string Cassie { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entire facility can hear the announcement.
        /// </summary>
        public bool IsGlobal { get; set; }

        /// <summary>
        /// Gets or sets the time, in seconds, to wait before triggering this phase.
        /// </summary>
        public float TriggerTime
        {
            get => triggerTime;
            set
            {
                if (value < 0)
                    value = 0;

                triggerTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the special action of the phase.
        /// </summary>
        public SpecialAction SpecialAction { get; set; } = SpecialAction.None;

        /// <summary>
        /// Executes the phase.
        /// </summary>
        public void Run()
        {
            foreach (Player player in Player.List)
            {
                float y = player.Position.y;
                if (IsGlobal || (y < 200f && y > -200.0f))
                {
                    if (Broadcast != null)
                        player.Broadcast(Broadcast);

                    player.PlayCassieAnnouncement(Cassie);
                }
            }

            if (SpecialAction == SpecialAction.Checkpoints)
                DoorEventOpenerExtension.TriggerAction(DoorEventOpenerExtension.OpenerEventType.DeconEvac);

            if (SpecialAction == SpecialAction.Lockdown)
                Map.StartDecontamination();
        }
    }
}