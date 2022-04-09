// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Exiled.Loader;
    using ScpDeathmatch.Configs;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets pairs of doors and the amount of time they should be locked for at the start of the round.
        /// </summary>
        [Description("Pairs of doors and the amount of time they should be locked for at the start of the round.")]
        public Dictionary<DoorType, float> DoorLocks { get; set; } = new()
        {
            { DoorType.GateA, 120f },
            { DoorType.GateB, 120f },
        };

        /// <summary>
        /// Gets or sets the folder containing miscellaneous config files.
        /// </summary>
        public string Folder { get; set; } = Path.Combine(Paths.Configs, "ScpDeathmatch");

        /// <summary>
        /// Gets or sets the configs for class selection.
        /// </summary>
        [YamlIgnore]
        public ClassSelectionConfig ClassSelection { get; set; }

        /// <summary>
        /// Gets or sets the configs for automated commands.
        /// </summary>
        [YamlIgnore]
        public CommandConfig Commands { get; set; }

        /// <summary>
        /// Gets or sets the configs for the custom items.
        /// </summary>
        [YamlIgnore]
        public CustomItemsConfig CustomItems { get; set; }

        /// <summary>
        /// Gets or sets the configs for the custom roles.
        /// </summary>
        [YamlIgnore]
        public CustomRolesConfig CustomRoles { get; set; }

        /// <summary>
        /// Gets or sets the configs related to the custom decontamination sequence.
        /// </summary>
        [YamlIgnore]
        public DecontaminationConfig Decontamination { get; set; }

        /// <summary>
        /// Gets or sets the configs related to disarming.
        /// </summary>
        [YamlIgnore]
        public DisarmingConfig Disarming { get; set; }

        /// <summary>
        /// Gets or sets the configs related to obtaining extra lives.
        /// </summary>
        [YamlIgnore]
        public ExtraLivesConfig ExtraLives { get; set; }

        /// <summary>
        /// Gets or sets the configs related to the micro healing.
        /// </summary>
        [YamlIgnore]
        public HealingMicroConfig HealingMicro { get; set; }

        /// <summary>
        /// Gets or sets the configs related to the omega warhead.
        /// </summary>
        [YamlIgnore]
        public OmegaWarheadConfig OmegaWarhead { get; set; }

        /// <summary>
        /// Gets or sets the configs for the <see cref="KillRewards.RewardManager"/>.
        /// </summary>
        [YamlIgnore]
        public RewardsConfig Rewards { get; set; }

        /// <summary>
        /// Gets or sets the configs for the stat broadcast.
        /// </summary>
        [YamlIgnore]
        public StatBroadcastConfig StatBroadcast { get; set; }

        /// <summary>
        /// Gets or sets the configs for the stats database.
        /// </summary>
        [YamlIgnore]
        public StatsDatabaseConfig StatsDatabase { get; set; }

        /// <summary>
        /// Gets or sets the configs for the <see cref="Managers.ZoneAnnouncer"/>.
        /// </summary>
        [YamlIgnore]
        public ZoneAnnouncerConfig ZoneAnnouncer { get; set; }

        /// <summary>
        /// Reloads all config files.
        /// </summary>
        public void Reload()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            foreach (PropertyInfo property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    if (!Attribute.IsDefined(property, typeof(YamlIgnoreAttribute)))
                        continue;

                    string path = Path.Combine(Folder, property.Name + ".yml");
                    if (!File.Exists(path))
                    {
                        property.SetValue(this, Activator.CreateInstance(property.PropertyType));
                        File.WriteAllText(path, Loader.Serializer.Serialize(property.GetValue(this)));
                        continue;
                    }

                    property.SetValue(this, Loader.Deserializer.Deserialize(File.ReadAllText(path), property.PropertyType));
                    File.WriteAllText(path, Loader.Serializer.Serialize(property.GetValue(this)));
                }
                catch (Exception e)
                {
                    Log.Error($"Error while attempting to reload config file '{property.Name}':\n{e.Message}");
                }
            }
        }
    }
}