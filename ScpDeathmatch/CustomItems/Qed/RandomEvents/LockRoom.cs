// -----------------------------------------------------------------------
// <copyright file="LockRoom.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.CustomItems.Qed.Enums;

    /// <inheritdoc />
    public class LockRoom : IRandomEvent
    {
        /// <inheritdoc />
        public string Name { get; set; } = nameof(LockRoom);

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public RangeType Range { get; set; } = RangeType.Neutral;

        /// <summary>
        /// Gets or sets the duration of the lockdown.
        /// </summary>
        [Description("The duration of the lockdown.")]
        public float Duration { get; set; } = 5f;

        /// <inheritdoc />
        public void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            Map.FindParentRoom(ev.Grenade.gameObject)?.LockDown(Duration, DoorLockType.AdminCommand);
        }
    }
}