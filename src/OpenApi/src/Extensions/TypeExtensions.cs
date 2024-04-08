// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.OpenApi;

internal static class TypeExtensions
{
    /// <summary>
    /// Gets the schema reference identifier for the given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to resolve a schema reference identifier for.</param>
    /// <returns>The schema reference identifier associated with <paramref name="type"/>.</returns>
    public static string GetSchemaReferenceId(this Type type)
    {
        if (!type.IsConstructedGenericType)
        {
            return type.Name.Replace("[]", "Array");
        }

        var prefix = type.GetGenericArguments()
            .Select(GetSchemaReferenceId)
            .Aggregate((previous, current) => previous + current);

        if (IsAnonymousType(type))
        {
            return prefix + "AnonymousType";
        }

        return prefix + type.Name.Split('`').First();
    }

    /// <summary>
    /// Determines whether the given <paramref name="type"/> is an anonymous type.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to check.</param>
    /// <returns><see langword="true"/> if <paramref name="type"/> is an anonymous type, <see langword="false"/> otherwise.</returns>
    public static bool IsAnonymousType(this Type type) =>
        type.GetTypeInfo().IsClass
            && type.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute))
            && !type.IsNested
            && type.Name.StartsWith("<>", StringComparison.Ordinal)
            && type.Name.Contains("__Anonymous");
}
