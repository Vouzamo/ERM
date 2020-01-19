using GraphQL.Types;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.Api.Graph.Types.Input
{
    public class FieldInputType : InputObjectGraphType<FieldDTO>
    {
        public FieldInputType()
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
