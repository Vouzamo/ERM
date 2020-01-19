using GraphQL.Types;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class FieldInterface : InterfaceGraphType<Field>
    {
        public FieldInterface()
        {
            Name = "Field";

            Field(field => field.Key);
            Field(field => field.Name);
            Field(field => field.Mandatory);
            Field(field => field.Enumerable);
            Field(field => field.Type);
        }
    }
}
