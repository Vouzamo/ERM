using GraphQL.Types;
using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Api.Graph.Types.Input;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Converters;
using Vouzamo.ERM.Common.Extensions;
using Vouzamo.ERM.Common.Models;
using Vouzamo.ERM.Common.Models.Validation;
using Vouzamo.ERM.CQRS;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.Api.Graph
{
    public class MyMutation : ObjectGraphType
    {
        public MyMutation(IMediator mediator, IConverter converter)
        {
            Name = "Mutation";

            FieldAsync<EdgeTypeGraphType>(
                "createEdgeType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<IdGraphType> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var name = context.GetArgument<string>("name");
                    var id = context.GetArgument<Guid?>("id");

                    return await mediator.Send(new CreateEdgeTypeCommand(name, id));
                }
            );

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

            FieldAsync<NodeTypeGraphType>(
                "createNodeType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<IdGraphType> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var name = context.GetArgument<string>("name");
                    var id = context.GetArgument<Guid?>("id");

                    return await mediator.Send(new CreateNodeTypeCommand(name, id));
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

            FieldAsync<NodeTypeGraphType>(
                "addFieldToNodeType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<FieldInputType>> { Name = "field" }
                ),
                resolve: async context =>
                {
                    var id = context.GetArgument<Guid>("id");
                    var field = context.GetArgument<FieldDTO>("field");
                    
                    return await mediator.Send(new NodeTypeAddFieldCommand(id, field));
                }
            );

            FieldAsync<JsonGraphType>(
                "nodeProperties",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "node" },
                    new QueryArgument<StringGraphType> { Name = "localization" },
                    new QueryArgument<NonNullGraphType<JsonGraphType>> { Name = "properties" }
                ),
                resolve: async context =>
                {
                    var id = context.GetArgument<Guid>("node");
                    var localization = context.GetArgument("localization", "default");
                    var properties = context.GetArgument<Dictionary<string, object>>("properties");

                    var localizationHierarchy = await mediator.Send(new LocalizationHierarchyCommand());

                    var nodes = await mediator.Send(new NodesByIdQuery(new List<Guid> { id }));

                    var results = new List<IValidationResult>();

                    if (nodes.TryGetValue(id, out var node))
                    {
                        var nodeTypes = await mediator.Send(new NodeTypesByIdQuery(new List<Guid> { node.Type }));

                        if(nodeTypes.TryGetValue(node.Type, out var nodeType))
                        {
                            var localizationChain = localizationHierarchy.FindDependencyChain(localization);

                            var editors = nodeType.Fields.AsEditors(node.Properties, localizationChain);

                            foreach(var editor in editors)
                            {
                                var key = editor.Field.Key;

                                if (!node.Properties.ContainsKey(key))
                                {
                                    node.Properties.Add(key, new LocalizedValue());
                                }

                                if (!editor.ReadOnly && properties.ContainsKey(key))
                                {
                                    node.Properties[key][localization] = properties[key];
                                }
                                else
                                {
                                    node.Properties[key].Remove(localization);
                                }

                                if(node.Properties[key].TryGetLocalizedValue(localizationChain, out var value))
                                {
                                    results.Add(editor.Field.Validate(value, converter));
                                }
                                else
                                {
                                    var mandatoryResult = new ValueValidationResult(!editor.Field.Mandatory);

                                    if(!mandatoryResult.Valid)
                                    {
                                        mandatoryResult.Messages.Add(new PropertyErrorValidationMessage(editor.Field.Key, $"Mandatory fields must provide a default value"));
                                    }

                                    results.Add(mandatoryResult);
                                }
                            }
                        }
                    }

                    var result = new AggregateValidationResult(results);

                    if(result.Valid)
                    {
                        await mediator.Send(new UpdateNodeCommand(node));
                    }

                    return result;
                }
            );
        }
    }
}
