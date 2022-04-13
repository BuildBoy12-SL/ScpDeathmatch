// -----------------------------------------------------------------------
// <copyright file="JammingCoin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using Interactables.Interobjects;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomItem(ItemType.Radio)]
    public class JammingCoin : CustomItem
    {
        private readonly Dictionary<Player, float> cooldowns = new();

        /// <inheritdoc />
        public override uint Id { get; set; } = 126;

        /// <inheritdoc />
        public override string Name { get; set; } = "Jamming Coin";

        /// <inheritdoc />
        public override string Description { get; set; } = "A coin that can be used to temporarily jam doors.";

        /// <inheritdoc />
        public override float Weight { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the minimum time, in seconds, between uses of this item.
        /// </summary>
        [Description("The minimum time, in seconds, between uses of this item.")]
        public float Cooldown { get; set; } = 30f;

        /// <summary>
        /// Gets or sets the duration of the lockdown on normal doors.
        /// </summary>
        [Description("The duration of the lockdown on normal doors.")]
        public float LockDuration { get; set; } = 10f;

        /// <summary>
        /// Gets or sets the duration of the lockdown on doors that require a keycard.
        /// </summary>
        [Description("The duration of the lockdown on doors that require a keycard.")]
        public float KeycardLockDuration { get; set; } = 6f;

        /// <summary>
        /// Gets or sets the duration of the lockdown on gates.
        /// </summary>
        [Description("The duration of the lockdown on gates.")]
        public float GateLockDuration { get; set; } = 2f;

        /// <inheritdoc />
        [YamlIgnore]
        public override SpawnProperties SpawnProperties { get; set; } = new();

        /// <inheritdoc />
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.Coin;

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            base.UnsubscribeEvents();
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem) || !ev.Door.IsOpen)
                return;

            if (cooldowns.TryGetValue(ev.Player, out float cooldown) && Time.time < cooldown)
                return;

            ev.Door.Lock(GetDuration(ev.Door), DoorLockType.AdminCommand);
            cooldowns[ev.Player] = Time.time + Cooldown;
        }

        private float GetDuration(Door door)
        {
            if (door.RequiredPermissions.RequiredPermissions == Interactables.Interobjects.DoorUtils.KeycardPermissions.None)
                return LockDuration;

            if (door.Base is PryableDoor)
                return GateLockDuration;

            return KeycardLockDuration;
        }
    }
}