﻿// -----------------------------------------------------------------------
// <copyright file="TimedCommandHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Managers
{
    using System.Collections.Generic;
    using Exiled.Events.EventArgs;
    using MEC;
    using ScpDeathmatch.Models;

    /// <summary>
    /// Handles configurable commands from <see cref="Configs.CommandConfig"/>.
    /// </summary>
    public class TimedCommandHandler : Subscribable
    {
        private readonly List<CoroutineHandle> coroutineHandles = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedCommandHandler"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public TimedCommandHandler(Plugin plugin)
            : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Subscribe()
        {
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }

        /// <inheritdoc />
        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (ConfiguredCommand command in Plugin.Config.Commands.RoundEnd)
                coroutineHandles.Add(command.Execute());
        }

        private void OnRoundStarted()
        {
            foreach (ConfiguredCommand command in Plugin.Config.Commands.RoundStart)
                coroutineHandles.Add(command.Execute());
        }

        private void OnWaitingForPlayers()
        {
            foreach (CoroutineHandle coroutineHandle in coroutineHandles)
            {
                if (coroutineHandle.IsRunning)
                    Timing.KillCoroutines(coroutineHandle);
            }

            coroutineHandles.Clear();
        }
    }
}