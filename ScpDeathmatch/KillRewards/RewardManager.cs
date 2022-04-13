// -----------------------------------------------------------------------
// <copyright file="RewardManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.KillRewards
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.KillRewards.Models;

    /// <summary>
    /// Manages rewards defined by <see cref="RewardRequirement"/>s.
    /// </summary>
    public class RewardManager
    {
        private readonly Plugin plugin;
        private readonly Dictionary<Player, (DamageType, HitboxType?)> cachedHitboxes = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="RewardManager"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public RewardManager(Plugin plugin) => this.plugin = plugin;

        private static Dictionary<ItemType, DamageType> ItemConversion { get; } = new()
        {
            { ItemType.GunCrossvec, DamageType.Crossvec },
            { ItemType.GunLogicer, DamageType.Logicer },
            { ItemType.GunRevolver, DamageType.Revolver },
            { ItemType.GunShotgun, DamageType.Shotgun },
            { ItemType.GunAK, DamageType.AK },
            { ItemType.GunCOM15, DamageType.Com15 },
            { ItemType.GunCOM18, DamageType.Com18 },
            { ItemType.GunFSP9, DamageType.Fsp9 },
            { ItemType.GunE11SR, DamageType.E11Sr },
            { ItemType.MicroHID, DamageType.MicroHid },
        };

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Shot += OnShot;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Shot -= OnShot;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (!plugin.Config.Rewards.IsEnabled ||
                ev.Killer is null ||
                !cachedHitboxes.TryGetValue(ev.Killer, out var tuple) ||
                plugin.Config.Rewards.Rewards.Count == 0)
                return;

            foreach (RewardRequirement rewardRequirement in plugin.Config.Rewards.Rewards)
            {
                if (rewardRequirement.Check(tuple.Item1, tuple.Item2))
                    rewardRequirement.Reward(ev.Killer, ev.Target);
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker is not null && !ev.Handler.Type.IsWeapon())
                cachedHitboxes[ev.Attacker] = (ev.Handler.Type, null);
        }

        private void OnShot(ShotEventArgs ev)
        {
            if (ev.Target is not null && ev.Shooter.CurrentItem is not null)
                cachedHitboxes[ev.Shooter] = (ItemConversion[ev.Shooter.CurrentItem.Type], ev.Hitbox._dmgMultiplier);
        }

        private void OnWaitingForPlayers() => cachedHitboxes.Clear();
    }
}