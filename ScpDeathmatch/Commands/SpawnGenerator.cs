// -----------------------------------------------------------------------
// <copyright file="SpawnGenerator.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Commands
{
    using System;
    using System.ComponentModel;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using MapGeneration.Distributors;
    using Mirror;
    using UnityEngine;

    /// <inheritdoc />
    public class SpawnGenerator : ICommand
    {
        private SpawnablesDistributorSettings[] settingsArray;

        /// <inheritdoc />
        public string Command { get; set; } = "spawngenerator";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "sgen", "sgenerator" };

        /// <inheritdoc />
        public string Description { get; set; } = "Spawns a generator.";

        /// <summary>
        /// Gets or sets the permission required to run this command.
        /// </summary>
        [Description("The permission required to run this command.")]
        public string RequiredPermission { get; set; } = "sd.generator";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = "You do not have permission to use this command.";
                return false;
            }

            if (Player.Get(sender) is not Player player)
            {
                response = "Only players can use this command.";
                return false;
            }

            settingsArray ??= Resources.LoadAll<MapGeneration.Distributors.SpawnablesDistributorSettings>(string.Empty);
            SpawnableStructure spawnableStructure = UnityEngine.Object.Instantiate(settingsArray[0].SpawnableStructures[7], player.Position, player.CameraTransform.rotation);
            spawnableStructure.transform.localScale = Vector3.one;
            NetworkServer.Spawn(spawnableStructure.gameObject);
            response = "Done.";
            return true;
        }
    }
}