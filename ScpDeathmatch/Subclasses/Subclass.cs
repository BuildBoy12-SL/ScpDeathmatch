// -----------------------------------------------------------------------
// <copyright file="Subclass.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.CustomItems.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using PlayerStatsSystem;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Represents a subclass.
    /// </summary>
    public abstract class Subclass
    {
        private readonly List<Player> trackedPlayers = new();

        /// <summary>
        /// Gets a collection of all registered custom roles.
        /// </summary>
        public static HashSet<Subclass> Registered { get; } = new();

        /// <summary>
        /// Gets a collection of players that have the subclass.
        /// </summary>
        public ReadOnlyCollection<Player> TrackedPlayers => trackedPlayers.AsReadOnly();

        /// <summary>
        /// Gets or sets the max <see cref="Player.Health"/> for the role.
        /// </summary>
        public abstract int MaxHealth { get; set; }

        /// <summary>
        /// Gets or sets the name of this role.
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of this role.
        /// </summary>
        public abstract string Description { get; set; }

        /// <summary>
        /// Gets or sets the CustomInfo of this role.
        /// </summary>
        public abstract string CustomInfo { get; set; }

        /// <summary>
        /// Gets or sets the badge to be applied to players that have the role.
        /// </summary>
        public abstract ConfiguredBadge Badge { get; set; }

        /// <summary>
        /// Gets or sets the badge to be applied to dead players that have the role.
        /// </summary>
        public virtual ConfiguredBadge DeadBadge { get; set; } = new("Dead", "silver");

        /// <summary>
        /// Gets or sets the abilities of the subclass.
        /// </summary>
        public virtual List<CustomAbility> CustomAbilities { get; set; } = new();

        /// <summary>
        /// Gets or sets the starting inventory for the role.
        /// </summary>
        public virtual List<string> Inventory { get; set; } = new();

        /// <summary>
        /// Gets a <see cref="CustomRole"/> by name.
        /// </summary>
        /// <param name="name">The name of the role to get.</param>
        /// <returns>The role, or <see langword="null"/> if it doesn't exist.</returns>
        public static Subclass Get(string name) => Registered.FirstOrDefault(subclass => subclass.Name == name);

        /// <summary>
        /// Gets a <see cref="CustomRole"/> by type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get.</param>
        /// <returns>The role, or <see langword="null"/> if it doesn't exist.</returns>
        public static Subclass Get(Type type) => Registered.FirstOrDefault(subclass => subclass.GetType() == type);

        /// <summary>
        /// Gets all of the subclasses a player has attached.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>The player's subclass, or null if one is not found.</returns>
        public static Subclass Get(Player player) => Registered.FirstOrDefault(subclass => subclass.Check(player));

        /// <summary>
        /// Registers all subclasses in the selected class.
        /// </summary>
        /// <param name="overrideClass">The class to pull the subclasses from.</param>
        /// <returns>The registered subclasses.</returns>
        public static IEnumerable<Subclass> RegisterSubclasses(object overrideClass)
        {
            Type type = overrideClass.GetType();
            if (!type.IsClass)
                throw new ArgumentException("OverrideClass must be a class.");

            List<Subclass> subclasses = new();
            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.PropertyType.IsSubclassOf(typeof(Subclass)))
                {
                    Subclass subclass = (Subclass)propertyInfo.GetValue(overrideClass) ?? Activator.CreateInstance(propertyInfo.PropertyType) as Subclass;
                    subclass?.TryRegister();
                    subclasses.Add(subclass);
                }
            }

            return subclasses;
        }

        /// <summary>
        /// Unregisters all subclasses.
        /// </summary>
        public static void UnregisterSubclasses()
        {
            foreach (Subclass subclass in Registered.ToList())
                subclass.TryUnregister();
        }

        /// <summary>
        /// Tries to register this subclass.
        /// </summary>
        /// <returns>True if the subclass registered properly.</returns>
        public bool TryRegister()
        {
            if (Registered.Contains(this))
            {
                Log.Warn($"Failed to register the subclass {Name} because it is already registered.");
                return false;
            }

            Registered.Add(this);
            Init();
            return true;
        }

        /// <summary>
        /// Tries to unregister this subclass.
        /// </summary>
        /// <returns>True if the role is subclass properly.</returns>
        public bool TryUnregister()
        {
            Destroy();
            if (Registered.Remove(this))
                return true;

            Log.Warn($"Cannot unregister {Name}, it hasn't been registered yet.");
            return false;
        }

        /// <summary>
        /// Initializes this role manager.
        /// </summary>
        public virtual void Init()
        {
            foreach (CustomAbility ability in CustomAbilities)
                ability.Init();

            SubscribeEvents();
        }

        /// <summary>
        /// Destroys this role manager.
        /// </summary>
        public virtual void Destroy()
        {
            foreach (CustomAbility ability in CustomAbilities)
                ability.Destroy();

            UnsubscribeEvents();
        }

        /// <summary>
        /// Handles setup of the role, including spawn location, inventory and registering event handlers.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to add the role to.</param>
        public virtual void AddRole(Player player)
        {
            if (Check(player))
                return;

            if (Get(player) is Subclass subclass)
                subclass.RemoveRole(player);

            player.Health = player.MaxHealth = MaxHealth;

            player.CustomInfo = CustomInfo;
            player.InfoArea &= ~PlayerInfoArea.Role;
            if (CustomAbilities is not null)
            {
                foreach (CustomAbility ability in CustomAbilities)
                    ability.AddAbility(player);
            }

            ShowMessage(player);
            RoleAdded(player);
            trackedPlayers.Add(player);
        }

        /// <summary>
        /// Removes the role from a specific player.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to remove the role from.</param>
        public virtual void RemoveRole(Player player)
        {
            if (!Check(player))
                return;

            trackedPlayers.Remove(player);
            player.CustomInfo = string.Empty;
            player.InfoArea |= PlayerInfoArea.Role;
            if (CustomAbilities is not null)
            {
                foreach (CustomAbility ability in CustomAbilities)
                    ability.RemoveAbility(player);
            }

            player.ReferenceHub.playerStats.GetModule<AhpStat>()._activeProcesses.Clear();
            RoleRemoved(player);
        }

        /// <summary>
        /// Checks if the given player has this role.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to check.</param>
        /// <returns>True if the player has this role.</returns>
        public bool Check(Player player) => trackedPlayers.Contains(player);

        /// <summary>
        /// Called when the role is initialized to setup internal events.
        /// </summary>
        protected virtual void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Player.Destroying += OnDestroying;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        }

        /// <summary>
        /// Called when the role is destroyed to unsubscribe internal event handlers.
        /// </summary>
        protected virtual void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Player.Destroying -= OnDestroying;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnChangingRole(ChangingRoleEventArgs)"/>
        protected virtual void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!Check(ev.Player))
                return;

            ConfiguredBadge badge = ev.NewRole.GetTeam() != Team.RIP ? Badge : DeadBadge;
            badge?.Apply(ev.Player);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnSpawned(ReferenceHub)"/>
        protected virtual void OnSpawned(SpawnedEventArgs ev)
        {
            if (!Check(ev.Player))
                return;

            if (Inventory is not null)
            {
                foreach (string item in Inventory)
                    TryAddItem(ev.Player, item);
            }

            ev.Player.Health = ev.Player.MaxHealth = MaxHealth;
        }

        /// <summary>
        /// Called after the role has been added to the player.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> the role was added to.</param>
        protected virtual void RoleAdded(Player player)
        {
        }

        /// <summary>
        /// Called 1 frame before the role is removed from the player.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> the role was removed from.</param>
        protected virtual void RoleRemoved(Player player)
        {
        }

        /// <summary>
        /// Shows the spawn message to the player.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to show the message to.</param>
        protected virtual void ShowMessage(Player player) => player.ShowHint(string.Format(Exiled.CustomRoles.CustomRoles.Instance.Config.GotRoleHint.Content, Name, Description), Exiled.CustomRoles.CustomRoles.Instance.Config.GotRoleHint.Duration);

        /// <summary>
        /// Tries to add an item to the player's inventory by name.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to try giving the item to.</param>
        /// <param name="itemName">The name of the item to try adding.</param>
        /// <returns>Whether or not the item was able to be added.</returns>
        protected bool TryAddItem(Player player, string itemName)
        {
            if (CustomItem.TryGet(itemName, out CustomItem customItem))
            {
                customItem.Give(player);
                return true;
            }

            if (Enum.TryParse(itemName, out ItemType type))
            {
                if (type.IsAmmo())
                    player.Ammo[type] = 100;
                else
                    player.AddItem(type);

                return true;
            }

            Log.Warn($"{Name}: {nameof(TryAddItem)}: {itemName} is not a valid ItemType or Custom Item name.");
            return false;
        }

        private void OnDestroying(DestroyingEventArgs ev)
        {
            if (Check(ev.Player))
                RemoveRole(ev.Player);
        }
    }
}