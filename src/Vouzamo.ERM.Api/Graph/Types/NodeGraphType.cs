using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;
using System;
using System.Linq;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS.Extensions;
using Vouzamo.ERM.Common.Models;
using Vouzamo.ERM.CQRS;
using Vouzamo.ERM.Common.Extensions;
using GraphQL;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class EditorGraphType : ObjectGraphType<Editor>
    {
        public EditorGraphType()
        {
            Field<ListGraphType<StringGraphType>>("localizationChain", resolve: context => context.Source.LocalizationChain);
            Field<ListGraphType<JsonGraphType>>("editors", resolve: context => context.Source.Editors);
            Field<JsonGraphType>("output", resolve: context => context.Source.BuildObject());
        }
    }

    public class NodeGraphType : ObjectGraphType<Node>
    {
        public NodeGraphType(IMediator mediator, IDataLoaderContextAccessor accessor)
        {
            Field(node => node.Id, type: typeof(IdGraphType));
            Field(node => node.Name);

            FieldAsync<EditorGraphType>(
                "properties",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "localization" }
                ),
                resolve: async context => {
                    var localization = context.GetArgument("localization", Constants.DefaultLocalization);

                    var localizationHierarchy = await mediator.Send(new LocalizationHierarchyCommand());
                    var localizationChain = localizationHierarchy.FindDependencyChain(localization);

                    var type = await mediator.Send(new ByIdQuery<Common.Type>(context.Source.Type));
                    var fields = await mediator.Send(new ExpandFieldsCommand(type));

                    var editor = fields.ToEditor(context.Source.Properties, localizationChain);

                    return editor;
                }
            );

            FieldAsync<TypeGraphType>(
                "type",
                resolve: async (context) => await mediator.Send(new ByIdQuery<Common.Type>(context.Source.Type))
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
