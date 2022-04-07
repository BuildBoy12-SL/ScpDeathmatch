// -----------------------------------------------------------------------
// <copyright file="PowerOutage.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.RandomEvents
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    /// <inheritdoc />
    public class PowerOutage : IRandomEvent
    {
        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public string Name { get; set; } = nameof(PowerOutage);

        /// <summary>
        /// Gets or sets a value indicating whether the outage should occur in the current room rather than by zones.
        /// </summary>
        [Description("Whether the outage should occur in the current room rather than by zones.")]
        public bool CurrentRoomOnly { get; set; } = false;

        /// <summary>
        /// Gets or sets the zones where the outage should occur.
        /// </summary>
        [Description("The zones where the outage should occur.")]
        public HashSet<ZoneType> Zones { get; set; } = new HashSet<ZoneType>
        {
            ZoneType.LightContainment,
            ZoneType.HeavyContainment,
            ZoneType.Entrance,
        };

        /// <summary>
        /// Gets or sets the duration of the power outage.
        /// </summary>
        [Description("The duration of the power outage.")]
        public float Duration { get; set; } = 10f;

        /// <inheritdoc />
        public void Action(ExplodingGrenadeEventArgs ev)
        {
            ev.TargetsToAffect.Clear();
            if (CurrentRoomOnly)
            {
                Map.FindParentRoom(ev.Grenade.gameObject)?.TurnOffLights(Duration);
                return;
            }

            Map.TurnOffAllLights(Duration, Zones);
        }
    }
}