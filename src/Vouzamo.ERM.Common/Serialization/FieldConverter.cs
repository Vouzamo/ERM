using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.Common.Serialization
{
    /// <summary>
    /// System.Text.Json requires a JsonConverter to deserialize polymophic types
    /// </summary>
    public class FieldConverter : JsonConverter<Field>
    {
        public FieldConverter()
        {

        }

        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(Field);
        }

        public override Field Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            using (var document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;

                if (document.RootElement.TryGetProperty("type", out JsonElement typeProperty))
                {
                    var typeValue = typeProperty.GetString();

                    switch (typeValue)
                    {
                        case "string": return JsonSerializer.Deserialize<StringField>(root.GetRawText(), options);
                        case "int": return JsonSerializer.Deserialize<IntegerField>(root.GetRawText(), options);
                        case "nested": return JsonSerializer.Deserialize<NestedField>(root.GetRawText(), options);
                    };
                }
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, Field value, JsonSerializerOptions options)
        {
            var serialized = JsonSerializer.Serialize(value, value.GetType(), options).Trim('"');

            writer.WriteStartObject();

            using (var doc = JsonDocument.Parse(serialized))
            {
                foreach (var property in doc.RootElement.EnumerateObject())
                {
                    property.WriteTo(writer);
                }
            }

            writer.WriteEndObject();
        }
    }
}
