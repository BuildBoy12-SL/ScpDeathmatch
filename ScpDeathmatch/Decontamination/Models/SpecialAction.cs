// -----------------------------------------------------------------------
// <copyright file="SpecialAction.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Decontamination.Models
{
    /// <summary>
    /// Indicates to the decontamination phase that special action should be taken.
    /// </summary>
    public enum SpecialAction
    {
        /// <summary>
        /// Indicates no action should be taken.
        /// </summary>
        None,

        /// <summary>
        /// Indicates checkpoints should be locked down.
        /// </summary>
        Checkpoints,

        /// <summary>
        /// Indicates lcz should be decontaminated.
        /// </summary>
        Lockdown,
    }
}