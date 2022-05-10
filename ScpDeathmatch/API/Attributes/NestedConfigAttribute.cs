// -----------------------------------------------------------------------
// <copyright file="NestedConfigAttribute.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.API.Attributes
{
    using System;

    /// <summary>
    /// An attribute to mark a property as a nested config.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NestedConfigAttribute : Attribute
    {
    }
}