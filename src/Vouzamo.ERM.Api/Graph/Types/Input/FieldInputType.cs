using GraphQL.Types;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.Api.Graph.Types.Input
{
    /// <summary>
    /// Once GraphQL.Net supports System.Text.Json we can use Field instead and receive a typed model (e.g. StringField)
    /// </summary>
    public class FieldInputType : InputObjectGraphType<FieldDTO>
    {
        public FieldInputType()
        {
            Name = "FieldInput";

            Field(field => field.Key);
            Field(field => field.Name);
            Field(field => field.Mandatory);
            Field(field => field.Enumerable);
            Field(field => field.Type);

            Field(field => field.MinLength, true);
            Field(field => field.MaxLength, true);

            Field(field => field.MinValue, true);
            Field(field => field.MaxValue, true);
        }
    }
}
