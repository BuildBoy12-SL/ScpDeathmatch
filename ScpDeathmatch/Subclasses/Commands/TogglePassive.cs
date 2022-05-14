// -----------------------------------------------------------------------
// <copyright file="TogglePassive.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.Subclasses.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using NorthwoodLib.Pools;
    using ScpDeathmatch.Subclasses.Interfaces;

    /// <inheritdoc />
    public class TogglePassive : ICommand
    {
        private static readonly Comparer<CustomAbility> AbilityComparer = Comparer<CustomAbility>.Create((ability1, ability2) => string.Compare(ability1.Name, ability2.Name, StringComparison.Ordinal));

        /// <inheritdoc />
        public string Command { get; set; } = "togglepassive";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "tpassive" };

        /// <inheritdoc />
        public string Description { get; set; } = "Toggles the user's passive abilities.";

        /// <summary>
        /// Gets or sets the response to give when the player has no available abilities to toggle.
        /// </summary>
        [Description("The response to give when the player has no available abilities to toggle.")]
        public string NoToggleablesResponse { get; set; } = "You have no passive abilities to toggle.";

        /// <summary>
        /// Gets or sets the response to give when the player specified an invalid ability to toggle.
        /// </summary>
        public string InvalidIndexResponse { get; set; } = "Invalid selection. Valid selections are 1-{0}.\n{1}";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Player.Get(sender) is not Player player)
            {
                response = "This command must be executed by a player.";
                return false;
            }

            Subclass subclass = Subclass.Get(player);
            if (subclass is null)
            {
                response = "You do not have a subclass.";
                return false;
            }

            SortedList<CustomAbility, IToggleablePassiveAbility> toggleableAbilities = new SortedList<CustomAbility, IToggleablePassiveAbility>(AbilityComparer);
            foreach (CustomAbility customAbility in subclass.CustomAbilities)
            {
                if (customAbility is IToggleablePassiveAbility toggleableAbility)
                    toggleableAbilities.Add(customAbility, toggleableAbility);
            }

            if (toggleableAbilities.Count == 0)
            {
                response = NoToggleablesResponse;
                return false;
            }

            KeyValuePair<CustomAbility, IToggleablePassiveAbility> toToggle;
            if (arguments.Count > 0)
            {
                if (int.TryParse(arguments.At(0), out int index) && index > 0 && index <= toggleableAbilities.Count)
                {
                    toToggle = toggleableAbilities.ElementAt(index - 1);
                }
                else
                {
                    response = string.Format(InvalidIndexResponse, toggleableAbilities.Count, FormatAbilities(toggleableAbilities));
                    return false;
                }
            }
            else
            {
                toToggle = toggleableAbilities.ElementAt(0);
            }

            return toToggle.Value.Toggle(player, out response);
        }

        private static string FormatAbilities(SortedList<CustomAbility, IToggleablePassiveAbility> toggleableAbilities)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            int i = 1;
            foreach (CustomAbility customAbility in toggleableAbilities.Keys)
            {
                stringBuilder.Append(i).Append(": ").AppendLine(customAbility.Name);
                i++;
            }

            return StringBuilderPool.Shared.ToStringReturn(stringBuilder);
        }
    }
}