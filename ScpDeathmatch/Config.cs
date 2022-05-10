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
    using ScpDeathmatch.API.Attributes;
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
        /// Gets or sets items that can open scp pedestal lockers.
        /// </summary>
        [Description("Items that can open scp pedestal lockers.")]
        public List<ItemType> CanOpenPedestal { get; set; } = new()
        {
            ItemType.KeycardScientist,
            ItemType.KeycardGuard,
        };

        /// <summary>
        /// Gets or sets a value indicating whether players will drop active Scp207 and Scp1853 effects as items when they die.
        /// </summary>
        [Description("Whether players will drop active Scp207 and Scp1853 effects as items when they die.")]
        public bool DropEffectsOnDeath { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the spawning of a <see cref="ItemType.GunCOM15"/> in <see cref="ZoneType.LightContainment"/> is blocked.
        /// </summary>
        [Description("Whether the spawning of a COM15 in Light Containment Zone is blocked.")]
        public bool PreventCom15Lcz { get; set; } = true;

        /// <summary>
        /// Gets or sets the time, in seconds, before all coins will be deleted from a round.
        /// </summary>
        [Description("The time, in seconds, before all coins will be deleted from a round.")]
        public float CoinDeletionDelay { get; set; } = 180f;

        /// <summary>
        /// Gets or sets the folder containing miscellaneous config files.
        /// </summary>
        [Description("The folder containing miscellaneous config files.")]
        public string Folder { get; set; } = Path.Combine(Paths.Configs, "ScpDeathmatch");

        /// <summary>
        /// Gets or sets the configs for body slamming.
        /// </summary>
        [YamlIgnore]
        public BodySlammingConfig BodySlamming { get; set; }

        /// <summary>
        /// Gets or sets the configs for class selection.
        /// </summary>
        [YamlIgnore]
        public ClassSelectionConfig ClassSelection { get; set; }

        /// <summary>
        /// Gets or sets the configs for general client commands.
        /// </summary>
        [YamlIgnore]
        public ClientCommandsConfig ClientCommands { get; set; }

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
        /// Gets or sets the configs related to generators.
        /// </summary>
        [YamlIgnore]
        public GeneratorsConfig Generators { get; set; }

        /// <summary>
        /// Gets or sets the configs related to the micro healing.
        /// </summary>
        [YamlIgnore]
        public HealingMicroConfig HealingMicro { get; set; }

        /// <summary>
        /// Gets or sets the configs related to the health system.
        /// </summary>
        [YamlIgnore]
        public HealthConfig Health { get; set; }

        /// <summary>
        /// Gets or sets the configs related to map generation.
        /// </summary>
        [YamlIgnore]
        public MapGenerationConfig MapGeneration { get; set; }

        /// <summary>
        /// Gets or sets the configs related to medical items.
        /// </summary>
        [YamlIgnore]
        public MedicalItemsConfig MedicalItems { get; set; }

        /// <summary>
        /// Gets or sets the configs for misc. commands.
        /// </summary>
        [YamlIgnore]
        public MiscCommandsConfig MiscCommands { get; set; }

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
        /// Gets or sets the configs related to the micro movement enhancement.
        /// </summary>
        [YamlIgnore]
        public SpeedyMicroConfig SpeedyMicro { get; set; }

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
        /// Gets or sets the configs for the custom roles.
        /// </summary>
        [YamlIgnore]
        [NestedConfig]
        public SubclassesConfig Subclasses { get; set; }

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
                if (!Attribute.IsDefined(property, typeof(YamlIgnoreAttribute)))
                    continue;

                if (Attribute.IsDefined(property, typeof(NestedConfigAttribute)))
                {
                    LoadNested(property);
                    continue;
                }

                string path = Path.Combine(Folder, property.Name + ".yml");
                LoadProperty(path, property, this);
            }
        }

        private void LoadNested(PropertyInfo property)
        {
            string directory = Path.Combine(Folder, property.Name);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            property.SetValue(this, Activator.CreateInstance(property.PropertyType));
            object value = property.GetValue(this);
            foreach (PropertyInfo nestedProperty in property.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string path = Path.Combine(directory, nestedProperty.Name + ".yml");
                LoadProperty(path, nestedProperty, value);
            }
        }

        private void LoadProperty(string path, PropertyInfo property, object parentClass)
        {
            try
            {
                object value = File.Exists(path)
                    ? Loader.Deserializer.Deserialize(File.ReadAllText(path), property.PropertyType)
                    : Activator.CreateInstance(property.PropertyType);

                property.SetValue(parentClass, value);
                File.WriteAllText(path, Loader.Serializer.Serialize(property.GetValue(parentClass)));
            }
            catch (Exception e)
            {
                Log.Error($"Error while attempting to reload config file '{property.Name}', defaults will be loaded instead!\n{e.Message}");
                property.SetValue(parentClass, Activator.CreateInstance(property.PropertyType));
            }
        }
    }
}