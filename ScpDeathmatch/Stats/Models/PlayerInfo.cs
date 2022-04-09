// -----------------------------------------------------------------------
// <copyright file="PlayerInfo.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Stats.Models
{
    public class PlayerInfo
    {
        public string UserId { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }
    }
}