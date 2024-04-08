// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

public class TypeExtensionsTests
{
    private delegate void TestDelegate(int x, int y);

    public static IEnumerable<object[]> GetSchemaReferenceId_Data =>
    [
        [typeof(Todo), "Todo"],
        [typeof(IEnumerable<Todo>), "TodoIEnumerable"],
        [typeof(TodoWithDueDate), "TodoWithDueDate"],
        [typeof(IEnumerable<TodoWithDueDate>), "TodoWithDueDateIEnumerable"],
        [(new { Id = 1 }).GetType(), "Int32AnonymousType"],
        [(new { Id = 1, Name = "Todo" }).GetType(), "Int32StringAnonymousType"],
        [typeof(IFormFile), "IFormFile"],
        [typeof(IFormFileCollection), "IFormFileCollection"],
        [typeof(Results<Ok<TodoWithDueDate>, Ok<Todo>>), "TodoWithDueDateOkTodoOkResults"],
        [typeof(TestDelegate), "TestDelegate"]
    ];

    public static IEnumerable<object[]> IsAnonymousType_Data =>
    [
        [typeof(Todo), false],
        [typeof(IEnumerable<Todo>), false],
        [typeof(TodoWithDueDate), false],
        [typeof(IEnumerable<TodoWithDueDate>), false],
        [(new { Id = 1 }).GetType(), true],
        [(new { Id = 1, Name = "Todo" }).GetType(), true],
        [typeof(IFormFile), false],
        [typeof(IFormFileCollection), false],
        [typeof(Results<Ok<TodoWithDueDate>, Ok<Todo>>), false],
        [typeof(TestDelegate), false]
    ];

    [Theory]
    [MemberData(nameof(GetSchemaReferenceId_Data))]
    public void GetSchemaReferenceId_Works(Type type, string referenceId)
        => Assert.Equal(referenceId, type.GetSchemaReferenceId());

    [Theory]
    [MemberData(nameof(IsAnonymousType_Data))]
    public void IsAnonymousType_Works(Type type, bool isAnonymousType)
        => Assert.Equal(isAnonymousType, type.IsAnonymousType());
}
