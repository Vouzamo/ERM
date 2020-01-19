using GraphQL.Types;
using MediatR;
using System;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Api.Graph.Types.Input;
using Vouzamo.ERM.CQRS;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.Api.Graph
{
    public class MyMutation : ObjectGraphType
    {
        public MyMutation(IMediator mediator)
        {
            Name = "Mutation";

            FieldAsync<NodeTypeGraphType>(
                "createNodeType",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<GuidGraphType> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var name = context.GetArgument<string>("name");
                    var id = context.GetArgument<Guid?>("id");

                    return await mediator.Send(new CreateNodeTypeCommand(name, id));
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
        }
    }
}
