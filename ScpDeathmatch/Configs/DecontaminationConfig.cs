// -----------------------------------------------------------------------
// <copyright file="DecontaminationConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    /// <summary>
    /// Handles configs related to the custom decontamination sequence.
    /// </summary>
    public class DecontaminationConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the custom decontamination sequence is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}