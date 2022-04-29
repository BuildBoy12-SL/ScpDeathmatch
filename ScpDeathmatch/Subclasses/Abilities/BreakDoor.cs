// -----------------------------------------------------------------------
// <copyright file="BreakDoor.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Interactables.Interobjects;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class BreakDoor : ActiveAbility
    {
        private readonly Dictionary<Player, int> uses = new();
        private readonly Dictionary<Player, Door> doors = new();

        /// <inheritdoc />
        public override string Name { get; set; } = "Break Door";

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        [YamlIgnore]
        public override float Duration { get; set; }

        /// <inheritdoc />
        public override float Cooldown { get; set; } = 30f;

        /// <summary>
        /// Gets or sets how many times the ability can be used per round.
        /// </summary>
        [Description("How many times the ability can be used per round.")]
        public int UsesPerRound { get; set; } = 5;

        /// <summary>
        /// Gets or sets the maximum distance a player can be from a door to use this ability.
        /// </summary>
        [Description("The maximum distance a player can be from a door to use this ability.")]
        public float MaximumDistance { get; set; } = 3f;

        /// <summary>
        /// Gets or sets a value indicating whether open doors can be broken.
        /// </summary>
        [Description("Whether open doors can be broken.")]
        public bool AffectOpenDoors { get; set; } = true;

        /// <inheritdoc />
        public override bool CanUseAbility(Player player, out string response)
        {
            if (uses.TryGetValue(player, out int count) && count >= UsesPerRound)
            {
                response = "You have used this ability the maximum amount of times.";
                return false;
            }

            Door closestDoor = ClosestDoor(player);
            if (closestDoor is null)
            {
                response = "You are too far away from a door to use this ability.";
                return false;
            }

            doors[player] = closestDoor;
            return base.CanUseAbility(player, out response);
        }

        /// <inheritdoc />
        protected override void AbilityUsed(Player player)
        {
            if (!uses.ContainsKey(player))
                uses.Add(player, 0);

            uses[player]++;
            if (doors.TryGetValue(player, out Door door))
                door.BreakDoor();
        }

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnWaitingForPlayers() => uses.Clear();

        private Door ClosestDoor(Player player)
        {
            float closest = MaximumDistance;
            Door closestObject = null;
            foreach (Door door in Door.List)
            {
                if (!door.IsBreakable || door.GameObject.GetComponentInParent<CheckpointDoor>())
                    continue;

                if (!AffectOpenDoors && door.IsOpen)
                    continue;

                float dist = Vector3.Distance(door.Position, player.Position);
                if (dist < closest)
                {
                    closest = dist;
                    closestObject = door;
                }
            }

            return closestObject;
        }
    }
}