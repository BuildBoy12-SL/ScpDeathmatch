// -----------------------------------------------------------------------
// <copyright file="DecontaminationConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using ScpDeathmatch.API.Interfaces;
    using ScpDeathmatch.Decontamination.Models;

    /// <summary>
    /// Handles configs related to the custom decontamination sequence.
    /// </summary>
    public class DecontaminationConfig : IConfigFile
    {
        /// <summary>
        /// Gets or sets a value indicating whether the custom decontamination sequence is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the phases of decontamination.
        /// </summary>
        public List<DecontaminationPhase> Phases { get; set; } = new()
        {
            new DecontaminationPhase
            {
                Broadcast = new Broadcast("Danger, Light Containment zone overall decontamination in 5 Minutes.", show: false),
                Cassie = "Danger . Light Containment zone overall decontamination in 5 Minutes",
                SuppressNoise = true,
                IsGlobal = false,
                TriggerTime = 10f,
            },
            new DecontaminationPhase
            {
                Broadcast = new Broadcast("Danger, Light Containment zone overall decontamination in 1 Minute.", show: false),
                Cassie = "Danger . Light Containment zone overall decontamination in 1 Minute",
                SuppressNoise = true,
                IsGlobal = false,
                TriggerTime = 240f,
            },
            new DecontaminationPhase
            {
                Broadcast = new Broadcast("Danger, Light Containment Zone overall decontamination in T-minus 30 seconds. All checkpoint doors have been permanently opened. Please evacuate immediately.", show: false),
                Cassie = "Danger . Light Containment Zone overall decontamination in 30 seconds",
                SuppressNoise = true,
                IsGlobal = false,
                TriggerTime = 30f,
                SpecialAction = SpecialAction.Checkpoints,
            },
            new DecontaminationPhase
            {
                Broadcast = new Broadcast("Light Containment Zone is locked down and ready for decontamination. The removal of organic substances has now begun.", show: false),
                Cassie = "Light Containment Zone is locked down and ready for decontamination",
                SuppressNoise = true,
                IsGlobal = true,
                TriggerTime = 30f,
                SpecialAction = SpecialAction.Lockdown,
            },
        };
    }
}