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
    using ScpDeathmatch.CustomItems;
    using ScpDeathmatch.Models;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the commands to be ran at the start of the round.
        /// </summary>
        [Description("The commands to be ran at the start of the round.")]
        public List<ConfiguredCommand> CommandList { get; set; } = new List<ConfiguredCommand>
        {
            new ConfiguredCommand
            {
                Command = "/command1",
                Delay = 0f,
            },
            new ConfiguredCommand
            {
                Command = "/command2",
                Delay = 5f,
            },
        };

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
        /// Gets or sets the configs for the <see cref="KillRewards.RewardManager"/>.
        /// </summary>
        public RewardsConfig Rewards { get; set; } = new RewardsConfig();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="CustomItems.SecondWind"/> class.
        /// </summary>
        public SecondWind SecondWind { get; set; } = new SecondWind();

        /// <summary>
        /// Gets or sets the configs for the <see cref="Managers.ZoneAnnouncer"/>.
        /// </summary>
        public ZoneAnnouncerConfig ZoneAnnouncer { get; set; } = new ZoneAnnouncerConfig();
    }
}