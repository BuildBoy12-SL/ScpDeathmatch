// -----------------------------------------------------------------------
// <copyright file="MedicalActions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem.Models
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using MEC;
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Subclasses;
    using UnityEngine;

    /// <summary>
    /// Represents the configured actions to apply when the corresponding medical item is consumed.
    /// </summary>
    public class MedicalActions
    {
        /// <summary>
        /// Gets or sets the health to immediately restore.
        /// </summary>
        public int InstantHealth { get; set; }

        /// <summary>
        /// Gets or sets the maximum health restored.
        /// </summary>
        public int InstantMaxHealth { get; set; }

        /// <summary>
        /// Gets or sets the amount of stamina to regenerate.
        /// </summary>
        public float RegeneratedStamina { get; set; }

        /// <summary>
        /// Gets or sets the health regeneration.
        /// </summary>
        public Regeneration Regeneration { get; set; }

        /// <summary>
        /// Gets or sets the maximum health regeneration.
        /// </summary>
        public Regeneration MaxHealthRegeneration { get; set; }

        /// <summary>
        /// Gets or sets the ahp to add.
        /// </summary>
        public ConfiguredAhp Ahp { get; set; }

        /// <summary>
        /// Gets or sets a collection of the effects to apply.
        /// </summary>
        public List<ConfiguredEffect> AddedEffects { get; set; }

        /// <summary>
        /// Gets or sets a collection of the effects to remove.
        /// </summary>
        public List<EffectType> RemovedEffects { get; set; }

        /// <summary>
        /// Gets or sets the duration to handcuff the user.
        /// </summary>
        public float DisarmDuration { get; set; }

        /// <summary>
        /// Applies all configured actions to the player.
        /// </summary>
        /// <param name="player">The player to apply the actions to.</param>
        public void ApplyTo(Player player)
        {
            player.MaxHealth = Mathf.Clamp(player.MaxHealth + InstantMaxHealth, 0, GetMaxHealth(player));
            player.Heal(InstantHealth);

            player.ReferenceHub.fpc.ModifyStamina(RegeneratedStamina);

            Regeneration?.ApplyTo(player);
            Ahp?.AddTo(player);
            if (RemovedEffects != null)
            {
                foreach (EffectType effect in RemovedEffects)
                {
                    player.DisableEffect(effect);
                }
            }

            if (AddedEffects != null)
            {
                foreach (ConfiguredEffect effect in AddedEffects)
                {
                    effect.Apply(player);
                }
            }

            if (DisarmDuration > 0f)
            {
                player.Handcuff();
                Timing.CallDelayed(DisarmDuration, () => player?.RemoveHandcuffs());
            }
        }

        private static int GetMaxHealth(Player player)
        {
            Subclass subclass = Subclass.Get(player);
            int maxHp = subclass?.MaxHealth ?? player.ReferenceHub.characterClassManager.CurRole.maxHP;
            int athleteMaxHp = Plugin.Instance.Config.Subclasses.Athlete.CurrentMaximumHealth(player);
            if (athleteMaxHp != -1)
                maxHp = athleteMaxHp;

            return maxHp;
        }
    }
}