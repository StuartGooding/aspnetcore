// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace Microsoft.AspNetCore.OpenApi;

/// <summary>
/// Supports managing elements that belong in the "components" section of
/// an OpenAPI document. In particular, this is the API that is used to
/// interact with the JSON schemas that are managed by a given OpenAPI document.
/// </summary>
internal sealed class OpenApiComponentService
{
    private readonly ConcurrentDictionary<string, OpenApiSchema> _schemas = new()
    {
        // Cache OpenAPI schemas for well-defined types in ASP.NET Core.
        [typeof(IFormFile).GetSchemaReferenceId()] = new OpenApiSchema { Type = "string", Format = "binary" },
        [typeof(IFormFileCollection).GetSchemaReferenceId()] = new OpenApiSchema
        {
            Type = "array",
            Items = new OpenApiSchema { Type = "string", Format = "binary" }
        },
    };

    internal OpenApiSchema GetOrCreateSchema(Type type)
    {
        var schemaId = type.GetSchemaReferenceId();
        return _schemas.GetOrAdd(schemaId, _ => CreateSchema());
    }

    internal static OpenApiSchema CreateSchema()
    {
        return new OpenApiSchema { Type = "string" };
    }
}
