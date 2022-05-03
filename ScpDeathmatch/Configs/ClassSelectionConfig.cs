// -----------------------------------------------------------------------
// <copyright file="ClassSelectionConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System.Collections.Generic;
    using ScpDeathmatch.Subclasses;

    /// <summary>
    /// Handles configs related to class selection in lobby.
    /// </summary>
    public class ClassSelectionConfig
    {
        /// <summary>
        /// Gets or sets a collection of items and their corresponding selection definition.
        /// </summary>
        public Dictionary<ItemType, SubclassSelection> Selections { get; set; } = new()
        {
            { ItemType.Adrenaline, new SubclassSelection("Athlete", "You've selected the Athlete subclass") },
            { ItemType.Medkit, new SubclassSelection("Brute", "You've selected the Brute subclass") },
            { ItemType.KeycardChaosInsurgency, new SubclassSelection("Insurgent", "You've selected the Insurgent subclass") },
            { ItemType.SCP1853, new SubclassSelection("Marksman", "You've selected the Marksman subclass") },
            { ItemType.Flashlight, new SubclassSelection("Recon", "You've selected the Recon subclass") },
            { ItemType.KeycardScientist, new SubclassSelection("Scavenger", "You've selected the Scavenger subclass") },
        };
    }
}