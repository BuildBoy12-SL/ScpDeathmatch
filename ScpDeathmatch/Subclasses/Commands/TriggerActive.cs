// -----------------------------------------------------------------------
// <copyright file="TriggerActive.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using NorthwoodLib.Pools;

    /// <inheritdoc />
    public class TriggerActive : ICommand
    {
        private static readonly Comparer<CustomAbility> AbilityComparer = Comparer<CustomAbility>.Create((ability1, ability2) => string.Compare(ability1.Name, ability2.Name, StringComparison.Ordinal));

        /// <inheritdoc />
        public string Command { get; set; } = "triggeractive";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "tactive", "active" };

        /// <inheritdoc />
        public string Description { get; set; } = "Triggers a subclasses active ability.";

        /// <summary>
        /// Gets or sets the response to give when the player has no available abilities to activate.
        /// </summary>
        [Description("The response to give when the player has no available abilities to activate.")]
        public string NoAbilitiesResponse { get; set; } = "You have no active abilities to engage.";

        /// <summary>
        /// Gets or sets the response to give when the player specified an invalid ability to toggle.
        /// </summary>
        [Description("The response to give when the player specified an invalid ability to toggle.")]
        public string InvalidIndexResponse { get; set; } = "Invalid selection. Valid selections are 1-{0}.\n{1}";

        /// <summary>
        /// Gets or sets the response to give when the player successfully activates an ability.
        /// </summary>
        [Description("The response to give when the player successfully activates an ability.")]
        public string AbilityUsedResponse { get; set; } = "Ability {0} used.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Player.Get(sender) is not Player player)
            {
                response = "This command must be executed by a player.";
                return false;
            }

            if (Subclass.Get(player) is not Subclass subclass)
            {
                response = "You do not have a subclass.";
                return false;
            }

            List<ActiveAbility> activeAbilities = new List<ActiveAbility>();
            foreach (CustomAbility customAbility in subclass.CustomAbilities)
            {
                if (customAbility is ActiveAbility activeAbility)
                    activeAbilities.Add(activeAbility);
            }

            activeAbilities.Sort(AbilityComparer);
            if (activeAbilities.Count == 0)
            {
                response = NoAbilitiesResponse;
                return false;
            }

            ActiveAbility toActivate;
            if (arguments.Count > 0)
            {
                if (int.TryParse(arguments.At(0), out int index) && index > 0 && index <= activeAbilities.Count)
                {
                    toActivate = activeAbilities[index - 1];
                }
                else
                {
                    response = string.Format(InvalidIndexResponse, activeAbilities.Count, FormatAbilities(activeAbilities));
                    return false;
                }
            }
            else
            {
                toActivate = activeAbilities[0];
            }

            if (!toActivate.CanUseAbility(player, out response))
                return false;

            toActivate.UseAbility(player);
            response = string.Format(AbilityUsedResponse, toActivate.Name);
            return true;
        }

        private static string FormatAbilities(List<ActiveAbility> activeAbilities)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            int i = 1;
            foreach (ActiveAbility activeAbility in activeAbilities)
            {
                stringBuilder.Append(i).Append(": ").AppendLine(activeAbility.Name);
                i++;
            }

            return StringBuilderPool.Shared.ToStringReturn(stringBuilder);
        }
    }
}