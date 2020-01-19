using GraphQL.Types;
using MediatR;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class NodeTypeGraphType : ObjectGraphType<NodeType>
    {
        public NodeTypeGraphType(IMediator mediator)
        {
            Field(nodeType => nodeType.Id, type: typeof(IdGraphType));
            Field(nodeType => nodeType.Name);
            Field<ListGraphType<FieldGraphType>>("fields", resolve: (context) => context.Source.Fields);

            FieldAsync<ListGraphType<NodeGraphType>>(
                name: "nodes",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take" },
                    new QueryArgument<IntGraphType> { Name = "skip" }
                ),
                resolve: async (context) => await mediator.Send(new NodesByNodeTypeQuery(context.Source.Id, context.GetArgument<int>("take"), context.GetArgument<int?>("skip").GetValueOrDefault()))
            );
        }
    }
}
