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

            // Expose characters
            FieldAsync<NodeTypeGraphType>(
                name: "nodeType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: async (context) => await mediator.Send(new NodeTypeByIdQuery(context.GetArgument<Guid>("id")))
            );
        }
    }
}
