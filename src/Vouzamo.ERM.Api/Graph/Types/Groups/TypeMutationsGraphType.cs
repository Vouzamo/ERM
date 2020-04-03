using GraphQL;
using GraphQL.Types;
using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Converters;
using Vouzamo.ERM.CQRS;
using Vouzamo.ERM.CQRS.Command;

namespace Vouzamo.ERM.Api.Graph.Types.Groups
{
    public class TypeMutationsGraphType : ObjectGraphType
    {
        public TypeMutationsGraphType(IMediator mediator, IConverter converter)
        {
            Name = "Types";

            FieldAsync<TypeGraphType>(
                "create",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<NonNullGraphType<TypeScopeEnumerationGraphType>> { Name = "scope" },
                    new QueryArgument<IdGraphType> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var name = context.GetArgument<string>("name");
                    var scope = context.GetArgument<TypeScope>("scope");
                    var id = context.GetArgument<Guid?>("id");

                    return await mediator.Send(new CreateTypeCommand(name, scope, id));
                }
            );

            FieldAsync<BooleanGraphType>(
                name: "rename",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }
                ),
                resolve: async context =>
                {
                    var id = context.GetArgument<Guid>("id");
                    var name = context.GetArgument<string>("name");

                    return await mediator.Send(new RenameCommand<Common.Type>(id, name));
                }
            );

            FieldAsync<TypeGraphType>(
                "updateType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<JsonGraphType>> { Name = "type" }
                ),
                resolve: async context =>
                {
                    var raw = context.GetArgument<object>("type");
                    var updated = converter.Convert<object, Common.Type>(raw);

                    return await mediator.Send(new UpdateCommand<Common.Type>(updated));
                }
            );

            FieldAsync<TypeGraphType>(
                "updateFields",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                    new QueryArgument<ListGraphType<JsonGraphType>> { Name = "fields" }
                ),
                resolve: async context =>
                {
                    var id = context.GetArgument<Guid>("id");
                    var raw = context.GetArgument<List<object>>("fields");

                    var fields = converter.Convert<List<object>, List<Field>>(raw);

                    return await mediator.Send(new UpdateFieldsCommand<Common.Type>(id, fields));
                }
            );
        }
    }
}
