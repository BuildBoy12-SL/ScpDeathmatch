// -----------------------------------------------------------------------
// <copyright file="ItemTranslations.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs.TranslationConfigs
{
    using System.Collections.Generic;
    using ScpDeathmatch.Configs.TranslationConfigs.Models;

    /// <inheritdoc />
    public class ItemTranslations : ITranslation<ItemType>
    {
        /// <inheritdoc />
        public Dictionary<ItemType, string> Translations { get; set; } = new()
        {
            { ItemType.GunCrossvec, "Cross Vector" },
            { ItemType.GunRevolver, "Revolver" },
            { ItemType.GunCOM15, "Com15" },
            { ItemType.GunCOM18, "Com18" },
            { ItemType.GunLogicer, "Logicer" },
            { ItemType.GunShotgun, "Shotgun" },
            { ItemType.GunAK, "AK" },
            { ItemType.GunFSP9, "FSP9" },
            { ItemType.GunE11SR, "E11-SR" },
        };
    }
}