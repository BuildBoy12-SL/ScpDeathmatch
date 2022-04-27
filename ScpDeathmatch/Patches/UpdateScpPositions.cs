// -----------------------------------------------------------------------
// <copyright file="UpdateScpPositions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1313
    using System.Collections.Generic;
    using Exiled.API.Features;
    using HarmonyLib;
    using MapGeneration;
    using ScpDeathmatch.Subclasses;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="Scp079PlayerScript.UpdateScpPositions"/> to update all player positions for the <see cref="Insurgent"/> subclass.
    /// </summary>
    [HarmonyPatch(typeof(Scp079PlayerScript), nameof(Scp079PlayerScript.UpdateScpPositions))]
    internal static class UpdateScpPositions
    {
        private static bool Prefix(Scp079PlayerScript __instance)
        {
            if (Player.Get(__instance.gameObject) is not Player player ||
                !Plugin.Instance.Config.Subclasses.Insurgent.Check(player))
                return true;

            __instance._serverIndicatorUpdateTimer += Time.deltaTime;
            if (__instance._serverIndicatorUpdateTimer < 1.29999995231628)
                return false;

            __instance._serverIndicatorUpdateTimer = 0.0f;
            if (__instance.currentCamera == null)
                return false;

            RoomIdentifier roomIdentifier1 = RoomIdUtils.RoomAtPosition(__instance.currentCamera.head.position);
            if (roomIdentifier1 == null)
                return false;

            FacilityZone zone = roomIdentifier1.Zone;
            List<Vector3> positions = new List<Vector3>();
            foreach (KeyValuePair<GameObject, ReferenceHub> allHub in ReferenceHub.GetAllHubs())
            {
                ReferenceHub referenceHub = allHub.Value;
                RoomIdentifier roomIdentifier2 = RoomIdUtils.RoomAtPositionRaycasts(referenceHub.PlayerCameraReference.position);
                if (roomIdentifier2 != null && roomIdentifier2.Zone == zone)
                    positions.Add(referenceHub.PlayerCameraReference.position);
            }

            if (positions.Count == 0)
            {
                if (__instance._sendIndicatorsOnce)
                    return false;

                __instance._sendIndicatorsOnce = true;
            }
            else
            {
                __instance._sendIndicatorsOnce = false;
            }

            __instance.TargetSetupIndicators(__instance.connectionToClient, positions);

            return false;
        }
    }
}