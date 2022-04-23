// -----------------------------------------------------------------------
// <copyright file="StatComponent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Stats.Components
{
    using AdvancedHints;
    using AdvancedHints.Enums;
    using Exiled.API.Features;
    using ScpDeathmatch.Stats.Models;
    using UnityEngine;

    /// <inheritdoc />
    public class StatComponent : MonoBehaviour
    {
        private Player player;
        private float globalTimer;

        private void Awake()
        {
            player = Player.Get(gameObject);
        }

        private void FixedUpdate()
        {
            if (Round.IsLobby)
                return;

            globalTimer += Time.deltaTime;
            if (globalTimer > 2f)
            {
                globalTimer = 0f;

                string healthString = player.IsAlive ? "Max Health: " + player.MaxHealth : string.Empty;
                if (ScpDeathmatch.Plugin.Instance.StatDatabase.TryGet(player, out PlayerInfo playerInfo))
                    player.ShowManagedHint("<align=left>" + healthString + "</align>                                                                               <align=right>" + playerInfo + "</align>", 2.2f, true, DisplayLocation.Bottom);
            }
        }
    }
}