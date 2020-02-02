using GraphQL;
using GraphQL.Types;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Api.Graph.Types.Groups;
using Vouzamo.ERM.Api.Graph.Types.Input;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Converters;
using Vouzamo.ERM.Common.Extensions;
using Vouzamo.ERM.Common.Models.Notifications;
using Vouzamo.ERM.Common.Models.Validation;
using Vouzamo.ERM.CQRS;
using Vouzamo.ERM.CQRS.Command;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.Api.Graph
{
    public class MyMutation : ObjectGraphType
    {
        public MyMutation(IMediator mediator, IConverter converter, INotificationManager manager)
        {
            Name = "Mutation";

            Field<TypeMutationsGraphType>("types", resolve: context => new { });

            FieldAsync<EdgeGraphType>(
                "createEdge",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "type" },
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "from" },
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "to" },
                    new QueryArgument<IdGraphType> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var type = context.GetArgument<Guid>("type");
                    var from = context.GetArgument<Guid>("from");
                    var to = context.GetArgument<Guid>("to");
                    var id = context.GetArgument<Guid?>("id");

                    return await mediator.Send(new CreateEdgeCommand(type, from, to, id));
                }
            );

            FieldAsync<NodeGraphType>(
                "createNode",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "type" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<IdGraphType> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var type = context.GetArgument<Guid>("type");
                    var name = context.GetArgument<string>("name");
                    var id = context.GetArgument<Guid?>("id");

                    return await mediator.Send(new CreateNodeCommand(type, name, id));
                }
            );

            FieldAsync<BooleanGraphType>(
                name: "renameNode",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }
                ),
                resolve: async context =>
                {
                    var id = context.GetArgument<Guid>("id");
                    var name = context.GetArgument<string>("name");

                    return await mediator.Send(new RenameCommand<Node>(id, name));
                }
            );

            FieldAsync<BooleanGraphType>(
                "nodeProperties",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "node" },
                    new QueryArgument<StringGraphType> { Name = "localization" },
                    new QueryArgument<NonNullGraphType<JsonGraphType>> { Name = "properties" }
                ),
                resolve: async context =>
                {
                    var id = context.GetArgument<Guid>("node");
                    var localization = context.GetArgument("localization", Constants.DefaultLocalization);
                    var properties = context.GetArgument<Dictionary<string, object>>("properties");

                    var results = new List<IValidationResult>();

                    var localizationHierarchy = await mediator.Send(new LocalizationHierarchyCommand());

                    var node = await mediator.Send(new ByIdQuery<Node>(id));

                    // Can't await this because of a bug: https://github.com/graphql-dotnet/graphql-dotnet/pull/1511
                    //var nodeTypeFailing = await mediator.Send(new ByIdQuery<Common.Type>(node.Type));

                    var nodeTypes = await mediator.Send(new ByIdsQuery<Common.Type>(new List<Guid> { node.Type }));

                    if (nodeTypes.TryGetValue(node.Type, out var nodeType))
                    {
                        var localizationChain = localizationHierarchy.FindDependencyChain(localization);

                        var editor = nodeType.Fields.AsEditor(node.Properties, localizationChain);

                        results.AddRange(editor.Editors.ValidateProperties(node, localization, localizationChain, converter, properties));
                    }

                    var result = new AggregateValidationResult(results);

                    if (result.Valid)
                    {
                        await mediator.Send(new UpdateCommand<Node>(node));
                    }

                    context.Errors.AddRange(result.Messages.Select(message => {
                        var path = context.Path.ToList();
                        path.Add("properties");
                        path.Add(message.Reference);
                        return new ExecutionError(message.Message) { Path = path };
                    }));

                    return result.Valid;
                }
            );

            FieldAsync<BooleanGraphType>(
                name: "addNotification",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "message" }
                ),
                resolve: async context =>
                {
                    var message = new SimpleNotificationMessage(context.GetArgument<string>("title"), context.GetArgument<string>("message"));

                    await manager.Notify(message);

                    return true;
                }
            );
        }
    }
}
