using GraphQL.Types;
using MediatR;
using Vouzamo.ERM.Api.Graph.Types.Input;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class EdgeGraphType : ObjectGraphType<Edge>
    {
        public EdgeGraphType(IMediator mediator)
        {
            Field(edge => edge.Id, type: typeof(IdGraphType));
            Field<JsonGraphType>("properties", resolve: context => context.Source.Properties);

            FieldAsync<EdgeTypeGraphType>(
                name: "type",
                resolve: async (context) => await mediator.Send(new EdgeTypeByIdQuery(context.Source.Type))
            );

            FieldAsync<NodeGraphType>(
                name: "from",
                resolve: async (context) => await mediator.Send(new NodeByIdQuery(context.Source.From))
            );

            FieldAsync<NodeGraphType>(
                name: "to",
                resolve: async (context) => await mediator.Send(new NodeByIdQuery(context.Source.To))
            );
        }
    }
}
