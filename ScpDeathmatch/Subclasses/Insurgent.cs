// -----------------------------------------------------------------------
// <copyright file="Insurgent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Roles;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using ScpDeathmatch.Models;
    using ScpDeathmatch.Subclasses.Abilities;
    using ScpDeathmatch.Subclasses.Models;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class Insurgent : Subclass
    {
        private CoroutineHandle levelsCoroutine;
        private byte currentLevel;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 100;

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(Insurgent);

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override string CustomInfo { get; set; }

        /// <inheritdoc />
        [YamlIgnore]
        public override ConfiguredBadge Badge { get; set; } = new(nameof(Insurgent), "emerald");

        /// <inheritdoc />
        public override ConfiguredBadge DeadBadge { get; set; } = new("Dead Insurgent", "nickel");

        /// <summary>
        /// Gets or sets the amount of time, in seconds, before this subclass can no longer respawn as an Scp079.
        /// </summary>
        [Description("The amount of time, in seconds, before this subclass can no longer respawn as an Scp079.")]
        public double RespawnCutoff { get; set; } = 450;

        /// <summary>
        /// Gets or sets a value indicating whether players of this subclass will be counted as alive when in Scp079 mode.
        /// </summary>
        public bool Count079Alive { get; set; } = false;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<string> Inventory { get; set; } = new();

        /// <inheritdoc />
        public override List<CustomAbility> CustomAbilities { get; set; } = new()
        {
            new StickyJamming(),
        };

        /// <summary>
        /// Gets or sets the times, in seconds, when Scp079s will be bumped a level.
        /// </summary>
        [Description("The times, in seconds, when Scp079s will be bumped a level.")]
        public List<float> UpgradeTimes { get; set; } = new()
        {
            150f,
            300f,
            450f,
            600f,
        };

        /// <summary>
        /// Gets or sets the types of insurgents that correspond to each role.
        /// </summary>
        public List<InsurgentType> InsurgentTypes { get; set; } = new()
        {
            new InsurgentType(RoleType.ClassD, new ConfiguredBadge("Starter Insurgent", "emerald")),
            new InsurgentType(RoleType.Scientist, new ConfiguredBadge("Weak Insurgent", "emerald")),
            new InsurgentType(RoleType.ChaosRifleman, new ConfiguredBadge("Strong Insurgent", "emerald")),
            new InsurgentType(RoleType.Scp079, new ConfiguredBadge("Meddling Insurgent", "emerald")),
        };

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Scp079.GainingExperience += OnGainingExperience;
            Exiled.Events.Handlers.Scp079.Recontained += OnRecontained;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Scp079.GainingExperience -= OnGainingExperience;
            Exiled.Events.Handlers.Scp079.Recontained -= OnRecontained;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            base.UnsubscribeEvents();
        }

        /// <inheritdoc />
        protected override void OnSpawned(SpawnedEventArgs ev)
        {
            if (!Check(ev.Player))
                return;

            ev.Player.Health = ev.Player.MaxHealth = MaxHealth;
            if (ev.Player.Role.Is(out Scp079Role scp079))
            {
                scp079.Level = currentLevel;
                ev.Player.Health = ev.Player.MaxHealth = ev.Player.ReferenceHub.characterClassManager.CurRole.maxHP;
            }

            InsurgentType insurgentType = InsurgentTypes.FirstOrDefault(type => type.Role == ev.Player.Role);
            if (insurgentType is null)
            {
                base.OnSpawned(ev);
                return;
            }

            ev.Player.ReferenceHub.serverRoles.Network_myText = insurgentType.Badge.Name;
            ev.Player.ReferenceHub.serverRoles.Network_myColor = insurgentType.Badge.Color;

            if (insurgentType.Inventory is not null)
            {
                foreach (string item in insurgentType.Inventory)
                    TryAddItem(ev.Player, item);
            }
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (!Check(ev.Target))
                return;

            if (ev.TargetOldRole == RoleType.Scp079 &&
                !Warhead.IsDetonated && !Recontainer.Base._alreadyRecontained)
            {
                ev.Target.Role.Type = RoleType.Scientist;
                return;
            }

            if (ev.TargetOldRole.GetSide() == Side.ChaosInsurgency && Round.ElapsedTime.TotalSeconds < RespawnCutoff &&
                !Warhead.IsDetonated && !Recontainer.Base._alreadyRecontained)
            {
                ev.Target.Role.Type = RoleType.Scp079;
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Target) && ev.Target.Role.Type == RoleType.Scp079 && ev.Handler.Type == DamageType.Crushed)
                ev.IsAllowed = false;
        }

        private void OnGainingExperience(GainingExperienceEventArgs ev)
        {
            if (!Check(ev.Player))
                return;

            ev.IsAllowed = false;
            if (ev.GainType is ExpGainType.DirectKill or ExpGainType.KillAssist or ExpGainType.PocketAssist)
                ev.Player.Role.Type = RoleType.ChaosRifleman;
        }

        private void OnRecontained(RecontainedEventArgs ev)
        {
            if (Check(ev.Target))
                ev.Target.Role.Type = RoleType.Scientist;
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (levelsCoroutine.IsRunning)
                Timing.KillCoroutines(levelsCoroutine);
        }

        private void OnRoundStarted()
        {
            if (levelsCoroutine.IsRunning)
                Timing.KillCoroutines(levelsCoroutine);

            levelsCoroutine = Timing.RunCoroutine(RunScp079LevelChecks());
        }

        private IEnumerator<float> RunScp079LevelChecks()
        {
            if (UpgradeTimes is null)
                yield break;

            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);

                int nextIndex = UpgradeTimes.Count - 1 >= currentLevel ? currentLevel : -1;
                if (nextIndex == -1)
                    break;

                if (Round.ElapsedTime.TotalSeconds < UpgradeTimes[nextIndex])
                    continue;

                currentLevel++;
                foreach (Player player in Player.List)
                {
                    if (!Check(player) || !player.Role.Is(out Scp079Role scp079))
                        continue;

                    scp079.Level = currentLevel;
                    scp079.MaxEnergy = scp079.Levels[currentLevel].maxMana;
                }
            }
        }
    }
}