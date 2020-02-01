using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;
using System;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class EdgeTraversalGraphType : ObjectGraphType<EdgeTraversal>
    {
        public EdgeTraversalGraphType(IMediator mediator, IDataLoaderContextAccessor accessor)
        {
            FieldAsync<EdgeGraphType>(
                name: "edge",
                resolve: async (context) => await mediator.Send(new ByIdQuery<Edge>(context.Source.Id))
            );

            FieldAsync<TypeGraphType>(
                name: "type",
                resolve: async (context) => await mediator.Send(new ByIdQuery<Common.Type>(context.Source.Type))
            );

            FieldAsync<NodeGraphType>(
                name: "node",
                resolve: async (context) => await mediator.Send(new ByIdQuery<Node>(context.Source.Node))
            );
        }
    }
}
