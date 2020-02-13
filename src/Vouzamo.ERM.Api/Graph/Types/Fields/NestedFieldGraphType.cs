using GraphQL.Types;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.Api.Graph.Types.Fields
{
    public class NestedFieldGraphType : FieldGraphType<NestedField>
    {
        public NestedFieldGraphType()
        {
            Name = "nested";

            Field(field => field.TypeId, type: typeof(IdGraphType));
        }
    }
}
