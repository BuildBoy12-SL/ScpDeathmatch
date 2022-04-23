// -----------------------------------------------------------------------
// <copyright file="RegenComponent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.HealthSystem
{
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using ScpDeathmatch.Configs;
    using UnityEngine;

    /// <summary>
    /// Handles the regeneration of players.
    /// </summary>
    public class RegenComponent : MonoBehaviour
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

            Exiled.Events.Handlers.Player.Hurting += OnHurting;
        }

        private void OnDestroy()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Timing.KillCoroutines(coroutineHandle);
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Target != player)
                return;

            lastHurt = Time.time;
            ev.Target.MaxHealth -= (int)(ev.Amount * (config.MaxHealthPercentage / 100f));
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