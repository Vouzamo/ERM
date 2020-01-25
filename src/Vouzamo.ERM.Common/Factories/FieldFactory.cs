using System;
using System.Text.Json;
using Vouzamo.ERM.Common.Serialization;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.Common.Factories
{
    public static class FieldFactory
    {
        public static Field CreateField(FieldDTO field)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            options.Converters.Add(new ObjectToPrimitiveConverter());
            options.Converters.Add(new FieldConverter());

            var json = JsonSerializer.Serialize(field, options);

            return JsonSerializer.Deserialize<Field>(json, options);
        }
    }
}
