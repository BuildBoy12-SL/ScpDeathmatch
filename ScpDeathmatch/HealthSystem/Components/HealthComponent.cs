﻿// -----------------------------------------------------------------------
// <copyright file="HealthComponent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem.Components
{
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using MEC;
    using PlayerStatsSystem;
    using ScpDeathmatch.Configs;
    using UnityEngine;

    /// <summary>
    /// Handles the regeneration of players.
    /// </summary>
    public class HealthComponent : MonoBehaviour
    {
        private CoroutineHandle coroutineHandle;
        private HealthConfig config;
        private Player player;
        private float lastHurt;

        private void Awake()
        {
            player = Player.Get(gameObject);
            config = Plugin.Instance.Config.Health;
            coroutineHandle = Timing.RunCoroutine(RunAttemptRegeneration());
            PlayerStats.OnAnyPlayerDamaged += OnAnyPlayerDamaged;
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
            player.MaxHealth -= (int)(amount * (config.MaxHealthPercentage / 100f));
        }

        private IEnumerator<float> RunAttemptRegeneration()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(config.RegenerationTick);
                if (player.IsDead)
                    continue;

                if (player.GetEffectActive<Invigorated>() || lastHurt + config.RegenDelay < Time.time)
                    player.Heal(player.MaxHealth * (config.RegenPercentage / 100f));
            }
        }
    }
}