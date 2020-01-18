using GraphQL.Types;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class NodeTypeGraphType : ObjectGraphType<NodeType>
    {
        public NodeTypeGraphType()
        {
            Field(nodeType => nodeType.Id, type: typeof(IdGraphType));
            Field(nodeType => nodeType.Name);
            Field<ListGraphType<FieldGraphType>>("fields", resolve: (context) => context.Source.Fields);
        }
    }
}
