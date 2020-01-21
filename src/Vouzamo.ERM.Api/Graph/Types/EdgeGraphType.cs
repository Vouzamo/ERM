using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;
using System;
using Vouzamo.ERM.Api.Graph.Types.Input;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class EdgeGraphType : ObjectGraphType<Edge>
    {
        public EdgeGraphType(IMediator mediator, IDataLoaderContextAccessor accessor)
        {
            Field(edge => edge.Id, type: typeof(IdGraphType));
            Field<JsonGraphType>("properties", resolve: context => context.Source.Properties);

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
                name: "from",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Node>("GetNodeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodesByIdQuery(ids));
                    });

                    return await loader.LoadAsync(context.Source.From);
                }
            );

            FieldAsync<NodeGraphType>(
                name: "to",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Node>("GetNodeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodesByIdQuery(ids));
                    });

                    return await loader.LoadAsync(context.Source.To);
                }
            );
        }
    }
}
