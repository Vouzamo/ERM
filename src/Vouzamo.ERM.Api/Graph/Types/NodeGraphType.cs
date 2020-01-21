using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;
using System;
using System.Linq;
using Vouzamo.ERM.Api.Graph.Types.Input;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class NodeGraphType : ObjectGraphType<Node>
    {
        public NodeGraphType(IMediator mediator, IDataLoaderContextAccessor accessor)
        {
            Field(node => node.Id, type: typeof(IdGraphType));
            Field(node => node.Name);
            Field<JsonGraphType>("properties", resolve: context => context.Source.Properties);

            FieldAsync<NodeTypeGraphType>(
                name: "type",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Node>("GetNodeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodesByIdQuery(ids));
                    });

                    return await loader.LoadAsync(context.Source.Type);
                }
            );

            FieldAsync<ListGraphType<EdgeTraversalGraphType>>(
                name: "traverse",
                arguments: new QueryArguments(
                    new QueryArgument<DirectionEnumerationGraphType> { Name = "direction" }
                ),
                resolve: async (context) => {
                    var direction = context.GetArgument<Direction?>("direction").GetValueOrDefault(Direction.Outbound);

                    var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid, Edge>("GetEdgesByNodeId", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodeEdgesByNodesQuery(ids, direction), cancellationToken);
                    });

                    var edges = await loader.LoadAsync(context.Source.Id);

                    var edgeTraversals = edges.Select(edge => new EdgeTraversal(edge.Id, edge.Type, direction.Equals(Direction.Outbound) ? edge.To : edge.From)).ToList();

                    return edgeTraversals;
                }
            );
        }
    }
}
