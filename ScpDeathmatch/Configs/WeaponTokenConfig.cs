// -----------------------------------------------------------------------
// <copyright file="WeaponTokenConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Configs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.CustomItems.API.Features;
    using ScpDeathmatch.API.Extensions;

    /// <summary>
    /// Handles configs for the <see cref="Managers.ArmoryPitManager"/>.
    /// </summary>
    public class WeaponTokenConfig
    {
        /// <summary>
        /// Gets or sets the list of possible rewards.
        /// </summary>
        [Description("The list of possible rewards. Accepts custom item names.")]
        public List<string> PossibleRewards { get; set; } = new()
        {
            $"{ItemType.GunRevolver}",
            $"{ItemType.MicroHID}",
            "BigIron",
        };

        /// <summary>
        /// Gets or sets the message to send a player when they receive their item.
        /// </summary>
        [Description("The message to send a player when they receive their item.")]
        public string GaveItem { get; set; } = "Gave you a {0} in exchange for your weapon token.";

        /// <summary>
        /// Gives a player a random reward from the <see cref="PossibleRewards"/> collection.
        /// </summary>
        /// <param name="player">The player to give the item to.</param>
        public void GiveRandom(Player player)
        {
            if (PossibleRewards is null || PossibleRewards.Count == 0)
                return;

            string name = PossibleRewards.Random();
            if (CustomItem.TryGive(player, name, false))
            {
                player.ShowHint(string.Format(GaveItem, name));
                return;
            }

            if (Enum.TryParse(name, true, out ItemType itemType))
            {
                player.AddItem(itemType);
                player.ShowHint(string.Format(GaveItem, name));
                return;
            }

            Log.Warn($"Failed to give a reward for a weapon token. '{name}' is not a valid custom item or item type.");
        }
    }
}