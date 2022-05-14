// -----------------------------------------------------------------------
// <copyright file="StickyJamming.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Subclasses.Insurgent.Abilities
{
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;
    using Interactables.Interobjects;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class StickyJamming : ActiveAbility
    {
        /// <inheritdoc />
        public override string Name { get; set; } = "Sticky Jamming";

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        [YamlIgnore]
        public override float Duration { get; set; }

        /// <inheritdoc />
        public override float Cooldown { get; set; } = 90f;

        /// <summary>
        /// Gets or sets the duration of the lockdown on normal doors.
        /// </summary>
        [Description("The duration of the lockdown on normal doors.")]
        public float LockDuration { get; set; } = 10f;

        /// <summary>
        /// Gets or sets the duration of the lockdown on doors that require a keycard.
        /// </summary>
        [Description("The duration of the lockdown on doors that require a keycard.")]
        public float KeycardLockDuration { get; set; } = 10f;

        /// <summary>
        /// Gets or sets the duration of the lockdown on gates.
        /// </summary>
        [Description("The duration of the lockdown on gates.")]
        public float GateLockDuration { get; set; } = 10f;

        /// <inheritdoc />
        public override bool CanUseAbility(Player player, out string response)
        {
            if (player.SessionVariables.ContainsKey("StickyJamming"))
            {
                response = "You already have a sticky coin queued!";
                return false;
            }

            return base.CanUseAbility(player, out response);
        }

        /// <inheritdoc />
        protected override void AbilityUsed(Player player)
        {
            player.SessionVariables.Add("StickyJamming", true);
        }

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
            if (!ev.Player.SessionVariables.ContainsKey("StickyJamming") || !ev.Door.IsOpen)
                return;

            ev.Door.Lock(GetDuration(ev.Door), DoorLockType.AdminCommand);
            ev.Player.SessionVariables.Remove("StickyJamming");
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