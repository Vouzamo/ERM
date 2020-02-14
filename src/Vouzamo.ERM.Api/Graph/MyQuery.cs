using GraphQL;
using GraphQL.Authorization;
using GraphQL.DataLoader;
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
            this.AuthorizeWith("Query");

            FieldAsync<BatchedResultsGraphType<Node, NodeGraphType>>(
                name: "nodeSearch",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "query" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "size" },
                    new QueryArgument<IntGraphType> { Name = "page" }
                ),
                resolve: async (context) => {
                    return await mediator.Send(
                        new NodesBySearchQuery(
                            context.GetArgument<string>("query"),
                            context.GetArgument<int>("size"),
                            context.GetArgument<int?>("page").GetValueOrDefault(1)
                        )
                    );
                }
            );

            FieldAsync<BatchedResultsGraphType<Common.Type, TypeGraphType>>(
                name: "typeSearch",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "query" },
                    new QueryArgument<TypeScopeEnumerationGraphType> { Name = "scope" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "size" },
                    new QueryArgument<IntGraphType> { Name = "page" }
                ),
                resolve: async (context) => {
                    return await mediator.Send(
                        new TypesBySearchQuery(
                            context.GetArgument<string>("query"),
                            context.GetArgument<TypeScope?>("scope"),
                            context.GetArgument<int>("size"),
                            context.GetArgument<int?>("page").GetValueOrDefault(1)
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
