// -----------------------------------------------------------------------
// <copyright file="ItemPair.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems.Qed.Models
{
    /// <summary>
    /// Represents an item and the amount of it to spawn.
    /// </summary>
    public class ItemPair
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemPair"/> class.
        /// </summary>
        public ItemPair()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemPair"/> class.
        /// </summary>
        /// <param name="item"><inheritdoc cref="Item"/></param>
        /// <param name="amount"><inheritdoc cref="Amount"/></param>
        public ItemPair(string item, int amount)
        {
            Item = item;
            Amount = amount;
        }

        /// <inheritdoc cref="ItemPair(string,int)"/>
        public ItemPair(ItemType item, int amount)
        {
            Item = item.ToString();
            Amount = amount;
        }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// Gets or sets the amount of the item.
        /// </summary>
        public int Amount { get; set; }
    }
}