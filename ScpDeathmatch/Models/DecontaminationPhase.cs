// -----------------------------------------------------------------------
// <copyright file="DecontaminationPhase.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Models
{
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Subtitles;

    /// <summary>
    /// Represents a phase in the decontamination sequence.
    /// </summary>
    public class DecontaminationPhase
    {
        /// <summary>
        /// Gets or sets the broadcast message to display.
        /// </summary>
        public Broadcast Broadcast { get; set; }

        /// <summary>
        /// Gets or sets the cassie message to play.
        /// </summary>
        public string Cassie { get; set; }

        /// <summary>
        /// Gets or sets the type of subtitle to display.
        /// </summary>
        public SubtitleType SubtitleType { get; set; }

        /// <summary>
        /// Gets or sets optional data to send with the subtitle.
        /// </summary>
        public string OptionalSubtitleData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entire facility can hear the announcement.
        /// </summary>
        public bool IsGlobal { get; set; }

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

                    player.Connection.Send(new SubtitleMessage(new[] { new SubtitlePart(SubtitleType, new[] { OptionalSubtitleData }) }));
                    player.PlayCassieAnnouncement(Cassie);
                }
            }
        }
    }
}