using GraphQL.Types;
using MediatR;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class EdgeTraversalGraphType : ObjectGraphType<EdgeTraversal>
    {
        public EdgeTraversalGraphType(IMediator mediator)
        {
            FieldAsync<NodeGraphType>(
                name: "edge",
                resolve: async (context) => await mediator.Send(new EdgeByIdQuery(context.Source.Id))
            );

            FieldAsync<EdgeTypeGraphType>(
                name: "type",
                resolve: async (context) => await mediator.Send(new EdgeTypeByIdQuery(context.Source.Type))
            );

            FieldAsync<NodeGraphType>(
                name: "node",
                resolve: async (context) => await mediator.Send(new NodeByIdQuery(context.Source.Node))
            );
        }
    }
}
