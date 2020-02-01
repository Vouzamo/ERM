﻿using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Exceptions;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph
{
    public class MyQuery : ObjectGraphType
    {
        public MyQuery(IMediator mediator, IDataLoaderContextAccessor accessor)
        {
            Name = "Query";

            FieldAsync<ListGraphType<NodeGraphType>>(
                name: "nodeSearch",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "query" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take" },
                    new QueryArgument<IntGraphType> { Name = "skip" }
                ),
                resolve: async (context) => {
                    return await mediator.Send(
                        new NodesBySearchQuery(
                            context.GetArgument<string>("query"),
                            context.GetArgument<int>("take"),
                            context.GetArgument<int?>("skip").GetValueOrDefault(0)
                        )
                    );
                }
            );

            FieldAsync<TypeGraphType>(
                name: "type",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }
                ),
                resolve: async (context) => await mediator.Send(new ByIdQuery<Common.Type>(context.GetArgument<Guid>("id")))
            );

            FieldAsync<NodeGraphType>(
                name: "node",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }
                ),
                resolve: async (context) => await mediator.Send(new ByIdQuery<Node>(context.GetArgument<Guid>("id")))
            );
        }
    }
}
