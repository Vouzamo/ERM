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
            FieldAsync<NodeGraphType>(
                name: "edge",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Edge>("GetEdgeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new EdgesByIdQuery(ids));
                    });

                    return await loader.LoadAsync(context.Source.Id);
                }
            );

            FieldAsync<EdgeTypeGraphType>(
                name: "type",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, EdgeType>("GetEdgeTypeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new EdgeTypesByIdQuery(ids));
                    });

                    return await loader.LoadAsync(context.Source.Type);
                }
            );

            FieldAsync<NodeGraphType>(
                name: "node",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Node>("GetNodeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodesByIdQuery(ids));
                    });

                    return await loader.LoadAsync(context.Source.Node);
                }
            );
        }
    }
}
