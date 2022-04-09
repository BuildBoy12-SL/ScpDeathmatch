// -----------------------------------------------------------------------
// <copyright file="StatComponent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Stats.Components
{
    using Exiled.API.Features;
    using UnityEngine;

    /// <inheritdoc />
    public class StatComponent : MonoBehaviour
    {
        private Player player;

        private void Awake()
        {
            player = Player.Get(gameObject);
        }
    }
}