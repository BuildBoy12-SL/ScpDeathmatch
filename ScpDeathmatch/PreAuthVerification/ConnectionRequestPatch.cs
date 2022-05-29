// -----------------------------------------------------------------------
// <copyright file="ConnectionRequestPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.PreAuthVerification
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using System.Text;
    using Cryptography;
    using HarmonyLib;
    using LiteNetLib;
    using LiteNetLib.Utils;
    using NorthwoodLib.Pools;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="CustomLiteNetLib4MirrorTransport.ProcessConnectionRequest"/> to ensure the validity of the request.
    /// </summary>
    [HarmonyPatch(typeof(CustomLiteNetLib4MirrorTransport), nameof(CustomLiteNetLib4MirrorTransport.ProcessConnectionRequest))]
    internal static class ConnectionRequestPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindIndex(instruction => instruction.OperandIs(Method(typeof(CustomLiteNetLib4MirrorTransport), nameof(CustomLiteNetLib4MirrorTransport.CheckIpRateLimit))));
            int insertionIndex = index - 1;
            int returnIndex = index + 2;

            Label returnLabel = (Label)newInstructions[returnIndex].operand;

            newInstructions.InsertRange(insertionIndex, new[]
            {
                new CodeInstruction(OpCodes.Ldloc_S, 8).MoveLabelsFrom(newInstructions[insertionIndex]),
                new(OpCodes.Ldarg_1),
                new(OpCodes.Call, Method(typeof(ConnectionRequestPatch), nameof(ValidateRequest))),
                new(OpCodes.Brfalse_S, returnLabel),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }

        private static bool ValidateRequest(byte[] array, ConnectionRequest request)
        {
            NetDataReader reader = new NetDataReader(request.Data.RawData);
            reader._position = 30;
            PreAuthModel preAuthData = PreAuthModel.ReadPreAuth(reader);
            if (preAuthData == null)
            {
                CustomLiteNetLib4MirrorTransport.RequestWriter.Reset();
                CustomLiteNetLib4MirrorTransport.RequestWriter.Put((byte)RejectionReason.Custom);
                CustomLiteNetLib4MirrorTransport.RequestWriter.Put("[ReddRoom]\nYour connection has been rejected as the 'PreAuth' data sent from your client appears to be invalid, please restart your game or run 'ar' in your client console, You can usually open the client console by pressing ` or ~");
                return false;
            }

            string s = Encoding.Default.GetString(array);
            if (!ECDSA.VerifyBytes($"{s};{preAuthData.Flags};{preAuthData.Region};{preAuthData.Expiration}", preAuthData.Signature, ServerConsole.PublicKey))
            {
                CustomLiteNetLib4MirrorTransport.RequestWriter.Reset();
                CustomLiteNetLib4MirrorTransport.RequestWriter.Put((byte)RejectionReason.Custom);
                CustomLiteNetLib4MirrorTransport.RequestWriter.Put("[ReddRoom]\nYour connection has been rejected as the 'PreAuth' data sent from your client appears to be invalid, please restart your game or run 'ar' in your client console, You can usually open the client console by pressing ` or ~");
                request.RejectForce(CustomLiteNetLib4MirrorTransport.RequestWriter);
                return false;
            }

            return true;
        }
    }
}