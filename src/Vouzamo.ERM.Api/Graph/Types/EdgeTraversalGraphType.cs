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
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Edge>("GetEdgeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new ByIdQuery<Edge>(ids));
                    });

                    return await loader.LoadAsync(context.Source.Id);
                }
            );

            FieldAsync<TypeGraphType>(
                name: "type",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Common.Type>("GetTypeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new ByIdQuery<Common.Type>(ids));
                    });

                    return await loader.LoadAsync(context.Source.Type);
                }
            );

            FieldAsync<NodeGraphType>(
                name: "node",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Node>("GetNodeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new ByIdQuery<Node>(ids));
                    });

                    return await loader.LoadAsync(context.Source.Node);
                }
            );
        }
    }
}
