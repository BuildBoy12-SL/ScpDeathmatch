// -----------------------------------------------------------------------
// <copyright file="MapEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.EventHandlers
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using ScpDeathmatch.Models;
    using MapHandlers = Exiled.Events.Handlers.Map;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Map"/>.
    /// </summary>
    public class MapEvents : Subscribable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapEvents"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public MapEvents(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            MapHandlers.SpawningItem += OnSpawningItem;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            MapHandlers.SpawningItem -= OnSpawningItem;
        }

        private void OnSpawningItem(SpawningItemEventArgs ev)
        {
            if (!Plugin.Config.PreventCom15Lcz || ev.Pickup.Type != ItemType.GunCOM15)
                return;

            Room room = Map.FindParentRoom(ev.Pickup.GameObject);
            if (room is not null && room.Zone == ZoneType.LightContainment)
                ev.IsAllowed = false;
        }
    }
}