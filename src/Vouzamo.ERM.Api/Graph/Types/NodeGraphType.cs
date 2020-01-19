using GraphQL.Types;
using MediatR;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class NodeGraphType : ObjectGraphType<Node>
    {
        public NodeGraphType(IMediator mediator)
        {
            Field(node => node.Id, type: typeof(IdGraphType));
            Field(node => node.Name);
            // Properties

            FieldAsync<NodeTypeGraphType>(
                name: "nodeType",
                resolve: async (context) => await mediator.Send(new NodeTypeByIdQuery(context.Source.Type))
            );
        }
    }
}
