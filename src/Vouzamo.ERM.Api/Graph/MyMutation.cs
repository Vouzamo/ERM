using GraphQL.Types;
using MediatR;
using System;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Api.Graph.Types.Input;
using Vouzamo.ERM.CQRS;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.Api.Graph
{
    public class MyMutation : ObjectGraphType
    {
        public MyMutation(IMediator mediator)
        {
            Name = "Mutation";

            FieldAsync<EdgeTypeGraphType>(
                "createEdgeType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<IdGraphType> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var name = context.GetArgument<string>("name");
                    var id = context.GetArgument<Guid?>("id");

                    return await mediator.Send(new CreateEdgeTypeCommand(name, id));
                }
            );

            FieldAsync<EdgeGraphType>(
                "createEdge",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "type" },
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "from" },
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "to" },
                    new QueryArgument<IdGraphType> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var type = context.GetArgument<Guid>("type");
                    var from = context.GetArgument<Guid>("from");
                    var to = context.GetArgument<Guid>("to");
                    var id = context.GetArgument<Guid?>("id");

                    return await mediator.Send(new CreateEdgeCommand(type, from, to, id));
                }
            );

            FieldAsync<NodeTypeGraphType>(
                "createNodeType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<IdGraphType> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var name = context.GetArgument<string>("name");
                    var id = context.GetArgument<Guid?>("id");

                    return await mediator.Send(new CreateNodeTypeCommand(name, id));
                }
            );

            FieldAsync<NodeGraphType>(
                "createNode",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "type" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<IdGraphType> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var type = context.GetArgument<Guid>("type");
                    var name = context.GetArgument<string>("name");
                    var id = context.GetArgument<Guid?>("id");

                    return await mediator.Send(new CreateNodeCommand(type, name, id));
                }
            );

            FieldAsync<NodeTypeGraphType>(
                "addFieldToNodeType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<FieldInputType>> { Name = "field" }
                ),
                resolve: async context =>
                {
                    var id = context.GetArgument<Guid>("id");
                    var field = context.GetArgument<FieldDTO>("field");
                    
                    return await mediator.Send(new NodeTypeAddFieldCommand(id, field));
                }
            );

            Field<JsonGraphType>(
                "rawTest",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<JsonGraphType>> { Name = "raw" }
                ),
                resolve: context =>
                {
                    var raw = context.GetArgument<object>("raw");

                    return raw;
                }
            );
        }
    }
}
