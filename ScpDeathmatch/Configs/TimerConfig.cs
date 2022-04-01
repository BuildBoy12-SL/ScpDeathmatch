// -----------------------------------------------------------------------
// <copyright file="TimerConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.ComponentModel;

    public class TimerConfig
    {
        /// <summary>
        /// Gets or sets the translation set in RespawnTimer so the timer knows what hint to modify.
        /// </summary>
        [Description("The translation set in RespawnTimer so the timer knows what hint to modify.")]
        public string YouWillRespawnIn { get; set; } = "<color=red>Autonuke begins in: </color>";
    }
}