// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Interfaces;
    using ScpDeathmatch.Configs;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets pairs of doors and the amount of time they should be locked for at the start of the round.
        /// </summary>
        [Description("Pairs of doors and the amount of time they should be locked for at the start of the round.")]
        public Dictionary<DoorType, float> DoorLocks { get; set; } = new Dictionary<DoorType, float>
        {
            { DoorType.GateA, 120f },
            { DoorType.GateB, 120f },
        };

        /// <summary>
        /// Gets or sets the configs for class selection.
        /// </summary>
        public ClassSelectionConfig ClassSelection { get; set; } = new ClassSelectionConfig();

        /// <summary>
        /// Gets or sets the configs for automated commands.
        /// </summary>
        public CommandConfig Commands { get; set; } = new CommandConfig();

        /// <summary>
        /// Gets or sets the configs for the custom items.
        /// </summary>
        public CustomItemsConfig CustomItems { get; set; } = new CustomItemsConfig();

        /// <summary>
        /// Gets or sets the configs for the custom roles.
        /// </summary>
        public CustomRolesConfig CustomRoles { get; set; } = new CustomRolesConfig();

        /// <summary>
        /// Gets or sets the configs related to the custom decontamination sequence.
        /// </summary>
        public DecontaminationConfig Decontamination { get; set; } = new DecontaminationConfig();

        /// <summary>
        /// Gets or sets the configs related to disarming.
        /// </summary>
        public DisarmingConfig Disarming { get; set; } = new DisarmingConfig();

        /// <summary>
        /// Gets or sets the configs related to obtaining extra lives.
        /// </summary>
        public ExtraLivesConfig ExtraLives { get; set; } = new ExtraLivesConfig();

        /// <summary>
        /// Gets or sets the configs related to the micro healing.
        /// </summary>
        public HealingMicroConfig HealingMicro { get; set; } = new HealingMicroConfig();

        /// <summary>
        /// Gets or sets the configs related to the omega warhead.
        /// </summary>
        public OmegaWarheadConfig OmegaWarhead { get; set; } = new OmegaWarheadConfig();

        /// <summary>
        /// Gets or sets the configs for the <see cref="KillRewards.RewardManager"/>.
        /// </summary>
        public RewardsConfig Rewards { get; set; } = new RewardsConfig();

        /// <summary>
        /// Gets or sets the configs for the stat broadcast.
        /// </summary>
        public StatBroadcastConfig StatBroadcast { get; set; } = new StatBroadcastConfig();

        /// <summary>
        /// Gets or sets the configs for the <see cref="Managers.ZoneAnnouncer"/>.
        /// </summary>
        public ZoneAnnouncerConfig ZoneAnnouncer { get; set; } = new ZoneAnnouncerConfig();
    }
}