﻿using GraphQL.DataLoader;
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

            FieldAsync<TypeGraphType>(
                name: "type",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader("GetTypeById", (Func<System.Collections.Generic.IEnumerable<Guid>, System.Threading.CancellationToken, System.Threading.Tasks.Task<System.Collections.Generic.IDictionary<Guid, Common.Type>>>)(async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new ByIdQuery<Common.Type>(ids));
                    }));

                    return await loader.LoadAsync(context.Source.Type);
                }
            );

            FieldAsync<NodeGraphType>(
                name: "from",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Node>("GetNodeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new ByIdQuery<Node>(ids));
                    });

                    return await loader.LoadAsync(context.Source.From);
                }
            );

            FieldAsync<NodeGraphType>(
                name: "to",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, Node>("GetNodeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new ByIdQuery<Node>(ids));
                    });

                    return await loader.LoadAsync(context.Source.To);
                }
            );
        }
    }
}
