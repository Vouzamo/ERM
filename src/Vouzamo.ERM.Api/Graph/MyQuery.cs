﻿using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;
using System;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph
{
    public class MyQuery : ObjectGraphType
    {
        public MyQuery(IMediator mediator, IDataLoaderContextAccessor accessor)
        {
            Name = "Query";

            FieldAsync<NodeTypeGraphType>(
                name: "nodeType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }
                ),
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, NodeType>("GetNodeTypeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodeTypesByIdQuery(ids));
                    });

                    return await loader.LoadAsync(context.GetArgument<Guid>("id"));
                }
            );

            FieldAsync<NodeGraphType>(
                name: "node",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }
                ),
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Node>("GetNodeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodesByIdQuery(ids));
                    });

                    return await loader.LoadAsync(context.GetArgument<Guid>("id"));
                }
            );
        }
    }
}
