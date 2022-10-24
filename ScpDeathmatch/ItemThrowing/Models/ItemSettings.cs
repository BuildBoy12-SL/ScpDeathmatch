// -----------------------------------------------------------------------
// <copyright file="ItemSettings.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.ItemThrowing.Models
{
    using Exiled.API.Features;
    using ScpDeathmatch.Models;
    using UnityEngine;

    /// <summary>
    /// Defines the contract for item settings.
    /// </summary>
    public abstract class ItemSettings
    {
        /// <summary>
        /// Gets or sets the damage to be applied on hit.
        /// </summary>
        public FloatRange Damage { get; set; }

        /// <summary>
        /// Gets or sets the health to be applied on hit.
        /// </summary>
        public FloatRange Heal { get; set; }

        /// <summary>
        /// Gets or sets the amount of artificial health to be applied on hit.
        /// </summary>
        public FloatRange AhpHeal { get; set; }

        /// <summary>
        /// Gets or sets the effects to be applied on hit.
        /// </summary>
        public ConfiguredEffect[] Effects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether prior effects should be cleared.
        /// </summary>
        public bool ClearEffects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item should be deleted when it hits a user.
        /// </summary>
        public bool ShouldDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether items can hit multiple people.
        /// </summary>
        public bool HitMultiple { get; set; }

        /// <summary>
        /// Applies the settings to a target player.
        /// </summary>
        /// <param name="target">The player to apply the settings to.</param>
        public void ApplyTo(Player target)
        {
            if (ClearEffects)
                target.DisableAllEffects();

            if (Heal != null)
                target.Heal(Heal.GetRandomValue());

            if (AhpHeal != null)
                target.ArtificialHealth = Mathf.Clamp(AhpHeal.GetRandomValue(), 0f, target.MaxArtificialHealth);

            if (Effects != null)
            {
                foreach (ConfiguredEffect effect in Effects)
                {
                    effect.Apply(target);
                }
            }

            if (Damage != null)
                target.Hurt(Damage.GetRandomValue());
        }
    }
}