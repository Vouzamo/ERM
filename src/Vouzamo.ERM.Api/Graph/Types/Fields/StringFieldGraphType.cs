using GraphQL.Types;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.Api.Graph.Types.Fields
{
    public class StringFieldGraphType : FieldGraphType<StringField>
    {
        public StringFieldGraphType()
        {
            Name = "string";

            Field(field => field.MinLength);
            Field(field => field.MaxLength);
        }
    }
}
