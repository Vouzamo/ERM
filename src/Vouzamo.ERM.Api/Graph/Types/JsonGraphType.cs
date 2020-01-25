using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using System.Collections.Generic;
using System.Text.Json;
using Vouzamo.ERM.Common.Serialization;

namespace Vouzamo.ERM.Api.Graph.Types
{
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
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            options.Converters.Add(new ObjectToPrimitiveConverter());
            options.Converters.Add(new FieldConverter());

            var json = JsonSerializer.Serialize<object>(value, options);

            return JsonSerializer.Deserialize<object>(json, options);
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
