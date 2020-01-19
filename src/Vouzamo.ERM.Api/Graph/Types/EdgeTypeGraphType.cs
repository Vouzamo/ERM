using GraphQL.Types;
using MediatR;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class EdgeTypeGraphType : ObjectGraphType<EdgeType>
    {
        public EdgeTypeGraphType(IMediator mediator)
        {
            Field(edgeType => edgeType.Id, type: typeof(IdGraphType));
            Field(edgeType => edgeType.Name);
            Field<ListGraphType<FieldInterface>>("fields", resolve: (context) => context.Source.Fields);

            // edges
        }
    }
}
