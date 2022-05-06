// -----------------------------------------------------------------------
// <copyright file="Cracked1853.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Abilities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using CustomPlayerEffects;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs;

    /// <inheritdoc />
    public class Cracked1853 : ActiveAbility
    {
        private readonly Dictionary<Player, byte> previousIntensity = new();

        /// <inheritdoc />
        public override string Name { get; set; } = "Cracked Scp1853";

        /// <inheritdoc />
        public override string Description { get; set; }

        /// <inheritdoc />
        public override float Duration { get; set; } = 10f;

        /// <inheritdoc />
        public override float Cooldown { get; set; } = 120f;

        /// <summary>
        /// Gets or sets the intensity of the applied <see cref="Scp1853"/> effect.
        /// </summary>
        [Description("The intensity of the applied Scp1853 effect.")]
        public byte Intensity { get; set; } = 10;

        /// <inheritdoc />
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            base.SubscribeEvents();
        }

        /// <inheritdoc />
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            base.UnsubscribeEvents();
        }

        /// <inheritdoc />
        protected override void AbilityUsed(Player player)
        {
            previousIntensity[player] = player.GetEffectIntensity<Scp1853>();
            player.ChangeEffectIntensity<Scp1853>(Intensity);
        }

        /// <inheritdoc />
        protected override void AbilityEnded(Player player)
        {
            if (!previousIntensity.TryGetValue(player, out byte intensity))
            {
                player.DisableEffect<Scp1853>();
                return;
            }

            player.ChangeEffectIntensity<Scp1853>(intensity);
            previousIntensity.Remove(player);
        }

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.SCP1853 && previousIntensity.ContainsKey(ev.Player))
            {
                previousIntensity[ev.Player]++;
                ev.Player.ChangeEffectIntensity<Scp1853>(Intensity);
            }
        }
    }
}