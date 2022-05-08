// -----------------------------------------------------------------------
// <copyright file="SubclassItem.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Items
{
    using System.Linq;
    using Exiled.API.Features.Items;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;

    /// <inheritdoc />
    public abstract class SubclassItem : CustomItem
    {
        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            base.UnsubscribeEvents();
        }

        private void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (Check(ev.Item))
                ev.IsAllowed = false;
        }

        private void OnDying(DyingEventArgs ev)
        {
            foreach (Item item in ev.Target.Items.ToList())
            {
                if (Check(item))
                    ev.Target.RemoveItem(item);
            }
        }
    }
}