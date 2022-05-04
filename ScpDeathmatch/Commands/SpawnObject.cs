// -----------------------------------------------------------------------
// <copyright file="SpawnObject.cs" company="Build">
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
    public class SpawnObject : ICommand
    {
        private SpawnablesDistributorSettings[] settingsArray;

        /// <inheritdoc />
        public string Command { get; set; } = "spawnobject";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "sobj", "sobject" };

        /// <inheritdoc />
        public string Description { get; set; } = "Spawns an object.";

        /// <summary>
        /// Gets or sets the permission required to run this command.
        /// </summary>
        [Description("The permission required to run this command.")]
        public string RequiredPermission { get; set; } = "sd.object";

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

            if (arguments.Count < 1)
            {
                response = "Usage: spawnobject <object>";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int id))
            {
                response = "Invalid object id. Could not parse the first argument to an integer.";
                return false;
            }

            settingsArray ??= Resources.LoadAll<MapGeneration.Distributors.SpawnablesDistributorSettings>(string.Empty);
            if (id < 0 || id >= settingsArray[0].SpawnableStructures.Length)
            {
                response = "Invalid object id. The object id must be between 0 and " + (settingsArray[0].SpawnableStructures.Length - 1) + ".";
                return false;
            }

            SpawnableStructure spawnableStructure = UnityEngine.Object.Instantiate(settingsArray[0].SpawnableStructures[id], player.Position, player.CameraTransform.rotation);
            spawnableStructure.transform.localScale = Vector3.one;
            NetworkServer.Spawn(spawnableStructure.gameObject);
            response = "Done.";
            return true;
        }
    }
}