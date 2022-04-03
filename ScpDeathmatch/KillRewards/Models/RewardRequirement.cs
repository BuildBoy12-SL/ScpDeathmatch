// -----------------------------------------------------------------------
// <copyright file="RewardRequirement.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.KillRewards.Models
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using MEC;

    /// <summary>
    /// Defines requirements and the rewards for meeting them.
    /// </summary>
    public class RewardRequirement
    {
        /// <summary>
        /// Gets or sets the weapon required to kill the victim.
        /// </summary>
        public DamageType DamageType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Hitbox"/> should be used in the <see cref="Check"/>.
        /// </summary>
        public bool SpecifyHitbox { get; set; }

        /// <summary>
        /// Gets or sets the location the final hit must land.
        /// </summary>
        public HitboxType Hitbox { get; set; }

        /// <summary>
        /// Gets or sets the rewards to be applied to those who meet the requirements.
        /// </summary>
        public Reward Config { get; set; }

        /// <summary>
        /// Checks whether the item and hitbox meet the requirements.
        /// </summary>
        /// <param name="damageType">The type of damage inflicted.</param>
        /// <param name="hitboxType">The hitbox where the hit landed.</param>
        /// <returns>Whether the check succeeded.</returns>
        public bool Check(DamageType damageType, HitboxType? hitboxType) => DamageType == damageType && (!SpecifyHitbox || Hitbox == hitboxType);

        /// <summary>
        /// Rewards the specified player.
        /// </summary>
        /// <param name="killer">The player to reward.</param>
        /// <param name="target">The player that was killed.</param>
        public void Reward(Player killer, Player target)
        {
            killer.ArtificialHealth += Config.AhpAmount;
            foreach (ConfiguredEffect effect in Config.Effects)
            {
                killer.EnableEffect(effect.Type, effect.Duration);
                killer.ChangeEffectIntensity(effect.Type, effect.Intensity, effect.Duration);
            }

            Map.Broadcast(
                Config.Broadcast.Duration,
                Config.Broadcast.Content
                    .Replace("%killer", killer.DisplayNickname ?? killer.Nickname)
                    .Replace("%victim", target.DisplayNickname ?? target.Nickname),
                Config.Broadcast.Type);

            Timing.RunCoroutine(RunRegen(killer));
        }

        private IEnumerator<float> RunRegen(Player player)
        {
            for (float i = 0; i < Config.HpRegenDuration; i++)
            {
                yield return Timing.WaitForSeconds(1f);
                if (player is null)
                    yield break;

                player.Heal(Config.HpRegenAmount / Config.HpRegenDuration);
            }
        }
    }
}