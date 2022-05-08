// -----------------------------------------------------------------------
// <copyright file="HealthComponent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using MEC;
    using PlayerStatsSystem;
    using ScpDeathmatch.Subclasses;
    using UnityEngine;

    /// <summary>
    /// Handles the regeneration of players.
    /// </summary>
    public class HealthComponent : MonoBehaviour
    {
        private CoroutineHandle coroutineHandle;
        private Config config;
        private Player player;
        private float lastHurt;

        /// <summary>
        /// Heals a players health and maximum health.
        /// </summary>
        public void Heal()
        {
            Subclass subclass = Subclass.GetSubclasses(player).FirstOrDefault();
            int maxHp = subclass?.MaxHealth ?? player.ReferenceHub.characterClassManager.CurRole.maxHP;

            int athleteMaxHp = config.Subclasses.Athlete.CurrentMaximumHealth(player);
            if (athleteMaxHp != -1)
                maxHp = athleteMaxHp;

            player.Health = player.MaxHealth = maxHp;
        }

        private void Awake()
        {
            player = Player.Get(gameObject);
            config = Plugin.Instance.Config;
            coroutineHandle = Timing.RunCoroutine(RunAttemptRegeneration());
            PlayerStats.OnAnyPlayerDamaged += OnAnyPlayerDamaged;
        }

        private void FixedUpdate()
        {
            if (player.Stamina.RemainingStamina > 0.025f)
                return;

            player.Stamina.RemainingStamina += config.Health.LfsStaminaAdded;
            int newMaxHealth = player.MaxHealth - config.Health.LfsHpRemoved;
            if (newMaxHealth <= 0)
            {
                player.Kill("Ran to death.");
                return;
            }

            player.MaxHealth = newMaxHealth;
            if (player.Health > player.MaxHealth)
            {
                player.Health = player.MaxHealth;
                lastHurt = Time.time;
            }
        }

        private void OnDestroy()
        {
            PlayerStats.OnAnyPlayerDamaged -= OnAnyPlayerDamaged;
            Timing.KillCoroutines(coroutineHandle);
        }

        private void OnAnyPlayerDamaged(ReferenceHub target, DamageHandlerBase damageHandler)
        {
            if (target != player.ReferenceHub)
                return;

            lastHurt = Time.time;
            if (damageHandler is not StandardDamageHandler standardDamageHandler)
                return;

            if (Plugin.Instance.Config.Subclasses.Brute.Check(player) && Plugin.Instance.Config.Subclasses.Brute.IgnoreMaxHpReduction)
                return;

            float amount = standardDamageHandler.DealtHealthDamage != 0 ? standardDamageHandler.DealtHealthDamage : standardDamageHandler.Damage;
            player.MaxHealth -= (int)(amount * (config.Health.MaxHealthPercentage / 100f));
        }

        private IEnumerator<float> RunAttemptRegeneration()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(config.Health.RegenerationTick);
                if (player.IsDead)
                    continue;

                if ((config.Health.InvigoratedBypassDelay && player.GetEffectActive<Invigorated>()) || lastHurt + config.Health.RegenDelay < Time.time)
                    player.Heal(player.MaxHealth * (config.Health.RegenPercentage / 100f));
            }
        }
    }
}