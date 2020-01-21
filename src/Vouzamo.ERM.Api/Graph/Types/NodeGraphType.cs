using GraphQL.Types;
using MediatR;
using System.Linq;
using Vouzamo.ERM.Api.Graph.Types.Input;
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
            Field<JsonGraphType>("properties", resolve: context => context.Source.Properties);

            FieldAsync<NodeTypeGraphType>(
                name: "type",
                resolve: async (context) => await mediator.Send(new NodeTypeByIdQuery(context.Source.Type))
            );

            FieldAsync<ListGraphType<EdgeTraversalGraphType>>(
                name: "traverse",
                arguments: new QueryArguments(
                    new QueryArgument<DirectionEnumerationGraphType> { Name = "direction" }
                ),
                resolve: async (context) => {
                    var direction = context.GetArgument<Direction?>("direction").GetValueOrDefault(Direction.Outbound);

                    var edges = await mediator.Send(new NodeEdgesQuery(context.Source.Id, direction));

                    var edgeTraversals = edges.Select(edge => new EdgeTraversal(edge.Id, edge.Type, direction.Equals(Direction.Outbound) ? edge.To : edge.From)).ToList();

                    return edgeTraversals;
                }
            );
        }
    }
}
