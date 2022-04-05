// -----------------------------------------------------------------------
// <copyright file="ReconSwitch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomItem(ItemType.Flashlight)]
    public class ReconSwitch : CustomItem
    {
        private readonly List<int> activeList = new List<int>();

        /// <inheritdoc />
        public override uint Id { get; set; } = 122;

        /// <inheritdoc />
        public override string Name { get; set; } = "Goggle Toggle";

        /// <inheritdoc />
        public override string Description { get; set; } = "Toggles goggles that mimic Scp939 vision";

        /// <inheritdoc />
        public override float Weight { get; set; } = 0f;

        /// <inheritdoc />
        [YamlIgnore]
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();

        /// <inheritdoc />
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.Flashlight;

        /// <summary>
        /// Gets or sets the type of 939 visuals.
        /// </summary>
        [Description("The type of 939 visuals.")]
        public byte Intensity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum distance that other players can be seen.
        /// </summary>
        public float MaximumDistance { get; set; } = 40f;

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Player.TogglingFlashlight += OnTogglingFlashlight;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Player.TogglingFlashlight -= OnTogglingFlashlight;
            base.UnsubscribeEvents();
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (activeList.Contains(ev.Player.Id))
            {
                ev.Player.DisableEffect(EffectType.Visuals939);
                activeList.Remove(ev.Player.Id);
            }
        }

        private void OnTogglingFlashlight(TogglingFlashlightEventArgs ev)
        {
            if (!Check(ev.Flashlight))
                return;

            ev.IsAllowed = false;
            if (activeList.Contains(ev.Player.Id))
            {
                ev.Player.DisableEffect(EffectType.Visuals939);
                activeList.Remove(ev.Player.Id);
                return;
            }

            ev.Player.EnableEffect(EffectType.Visuals939);
            ev.Player.ChangeEffectIntensity(EffectType.Visuals939, Intensity);
            activeList.Add(ev.Player.Id);
        }
    }
}