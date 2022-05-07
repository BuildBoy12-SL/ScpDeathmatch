// -----------------------------------------------------------------------
// <copyright file="ProcessConsoleQuery.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Patches
{
#pragma warning disable SA1118
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using RemoteAdmin;
    using ScpDeathmatch.Models;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="QueryProcessor.ProcessGameConsoleQuery"/> to implement <see cref="Configs.ClientCommandsConfig.CustomCommands"/>.
    /// </summary>
    [HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.ProcessGameConsoleQuery))]
    internal static class ProcessConsoleQuery
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label retLabel = generator.DefineLabel();
            newInstructions.InsertRange(0, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(ProcessConsoleQuery), nameof(TryRunCommand))),
                new CodeInstruction(OpCodes.Brtrue_S, retLabel),
            });

            newInstructions[newInstructions.Count - 1].labels.Add(retLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }

        private static bool TryRunCommand(QueryProcessor sender, string query)
        {
            ConsoleCommand clientCommand = Plugin.Instance.Config.ClientCommands.CustomCommands.FirstOrDefault(cmd => string.Equals(cmd.Command, query, StringComparison.OrdinalIgnoreCase));
            if (clientCommand is null)
                return false;

            sender.GCT.SendToClient(sender.connectionToClient, clientCommand.Response, clientCommand.Color);
            return true;
        }
    }
}