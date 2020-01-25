using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using Vouzamo.ERM.Api.Graph.Types.Input;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Extensions;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class NodeGraphType : ObjectGraphType<Node>
    {
        public NodeGraphType(IMediator mediator, IDataLoaderContextAccessor accessor)
        {
            Field(node => node.Id, type: typeof(IdGraphType));
            Field(node => node.Name);

            FieldAsync<JsonGraphType>(
                "properties",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "localization" }
                ),
                resolve: async context => {
                    var localization = context.GetArgument("localization", "default");

                    var localizationHierarchy = await mediator.Send(new LocalizationHierarchyCommand());
                    var localizationChain = localizationHierarchy.FindDependencyChain(localization);

                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, NodeType>("GetNodeTypeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodeTypesByIdQuery(ids));
                    });

                    var type = await loader.LoadAsync(context.Source.Type);

                    var editors = type.Fields.AsEditors(context.Source.Properties, localizationChain);

                    return editors.AsValues();
                }
            );

            FieldAsync<JsonGraphType>(
                "editor",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "localization" }
                ),
                resolve: async context => {
                    var localization = context.GetArgument("localization", "default");

                    var localizationHierarchy = await mediator.Send(new LocalizationHierarchyCommand());
                    var localizationChain = localizationHierarchy.FindDependencyChain(localization);

                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, NodeType>("GetNodeTypeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodeTypesByIdQuery(ids));
                    });

                    var type = await loader.LoadAsync(context.Source.Type);

                    return type.Fields.AsEditors(context.Source.Properties, localizationChain);
                }
            );

            FieldAsync<NodeTypeGraphType>(
                "type",
                resolve: async (context) => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Guid, NodeType>("GetNodeTypeById", async (ids, cancellationToken) =>
                    {
                        return await mediator.Send(new NodeTypesByIdQuery(ids));
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
