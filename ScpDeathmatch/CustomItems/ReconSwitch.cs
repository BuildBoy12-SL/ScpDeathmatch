// -----------------------------------------------------------------------
// <copyright file="ReconSwitch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.CustomItems
{
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    [CustomItem(ItemType.Flashlight)]
    public class ReconSwitch : CustomItem
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 122;

        /// <inheritdoc />
        public override string Name { get; set; } = "Goggle Toggle";

        /// <inheritdoc />
        public override string Description { get; set; } = "Toggles goggles that mimic Scp939 vision";

        /// <inheritdoc />
        public override float Weight { get; set; } = 0f;

        /// <inheritdoc />
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();

        /// <inheritdoc />
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.Flashlight;

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.TogglingFlashlight += OnTogglingFlashlight;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.TogglingFlashlight -= OnTogglingFlashlight;
            base.UnsubscribeEvents();
        }

        private void OnTogglingFlashlight(TogglingFlashlightEventArgs ev)
        {
            if (!Check(ev.Flashlight))
                return;

            ev.IsAllowed = false;
        }
    }
}