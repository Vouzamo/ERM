using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;
using System;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class NodeTypeGraphType : ObjectGraphType<NodeType>
    {
        public NodeTypeGraphType(IMediator mediator, IDataLoaderContextAccessor accessor)
        {
            Field(nodeType => nodeType.Id, type: typeof(IdGraphType));
            Field(nodeType => nodeType.Name);
            Field<ListGraphType<FieldInterface>>("fields", resolve: (context) => context.Source.Fields);

            FieldAsync<ListGraphType<NodeGraphType>>(
                name: "nodes",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take" },
                    new QueryArgument<IntGraphType> { Name = "skip" }
                ),
                resolve: async (context) =>
                {
                    var take = context.GetArgument<int>("take");
                    var skip = context.GetArgument<int?>("skip").GetValueOrDefault();

                    var loader = accessor.Context.GetOrAddCollectionBatchLoader<Guid, Node>("GetNodesByNodeTypeId", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodesByNodeTypesQuery(ids, take, skip), cancellationToken);
                    });

                    return await loader.LoadAsync(context.Source.Id);
                }
            );
        }
    }
}
