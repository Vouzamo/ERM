using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using System.Collections.Generic;
using Vouzamo.ERM.Common.Converters;

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
        protected IConverter Converter { get; }

        public JsonGraphType(IConverter converter)
        {
            Converter = converter;

            Name = "Json";
        }

        /// <summary>
        /// Ideally we should just return the value as-is but since we can't use our System.Text.Json (and options) in GraphQL we would otherwise get ProperCasePropertyNames
        /// </summary>
        /// <param name="value">The object to be serialized by GraphQL</param>
        /// <returns></returns>
        public override object Serialize(object value)
        {
            var converted = Converter.Convert<object, object>(value);

            return converted;
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
                return value.As<object>();
            }
        }
    }
}
