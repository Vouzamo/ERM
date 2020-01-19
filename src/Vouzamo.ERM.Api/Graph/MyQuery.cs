using GraphQL.Types;
using MediatR;
using System;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph
{
    public class MyQuery : ObjectGraphType
    {
        public MyQuery(IMediator mediator)
        {
            Name = "Query";

            FieldAsync<NodeTypeGraphType>(
                name: "nodeType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async (context) => await mediator.Send(new NodeTypeByIdQuery(context.GetArgument<Guid>("id")))
            );

            FieldAsync<NodeGraphType>(
                name: "node",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async (context) => await mediator.Send(new NodeByIdQuery(context.GetArgument<Guid>("id")))
            );
        }
    }
}
