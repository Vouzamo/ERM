using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vouzamo.ERM.Common.Serialization
{
    /// <summary>
    /// System.Text.Json requires a JsonConverter to deserialize primitive types
    /// </summary>
    public class ObjectToPrimitiveConverter : JsonConverter<object>
    {
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.True)
            {
                return true;
            }

            if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDouble();
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                if (reader.TryGetDateTime(out DateTime datetime))
                {
                    // If an offset was provided, use DateTimeOffset.
                    if (datetime.Kind == DateTimeKind.Local)
                    {
                        if (reader.TryGetDateTimeOffset(out DateTimeOffset datetimeOffset))
                        {
                            return datetimeOffset;
                        }
                    }

                    return datetime;
                }

                return reader.GetString();
            }

            // Use JsonElement as fallback.
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                return document.RootElement.Clone();
            }
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new InvalidOperationException("Should not get here.");
        }
    }
}
