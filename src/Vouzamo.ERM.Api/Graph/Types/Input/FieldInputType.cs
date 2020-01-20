using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using System.Collections.Generic;
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

    public class JsonGraphTypeConverter : IAstFromValueConverter
    {
        public bool Matches(object value, IGraphType type)
        {
            return type.Name == "Json";
        }

        public IValue Convert(object value, IGraphType type)
        {
            return new JsonGraphValue(value as Dictionary<string, object>);
        }
    }

    public class JsonGraphValue : ValueNode<Dictionary<string, object>>
    {
        public JsonGraphValue(Dictionary<string, object> value)
        {
            Value = value;
        }

        protected override bool Equals(ValueNode<Dictionary<string, object>> node)
        {
            return Value == node.Value;
        }
    }

    public class JsonGraphType : ScalarGraphType
    {
        public JsonGraphType()
        {
            Name = "Json";
        }

        public override object Serialize(object value)
        {
            return ParseValue(value);
        }

        public override object ParseValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            return value;
        }

        public override object ParseLiteral(IValue value)
        {
            if (value is JsonGraphValue)
            {
                return value.Value;
            }
            else
            {
                var tv = value as JsonGraphTypeConverter;
                return tv?.GetValue();
            }
        }
    }
}
