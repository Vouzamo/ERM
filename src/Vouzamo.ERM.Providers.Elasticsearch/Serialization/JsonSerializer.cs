using Elasticsearch.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common.Serialization;

namespace Vouzamo.ERM.Providers.Elasticsearch.Serialization
{
    public class SystemTextJsonSerializer : IElasticsearchSerializer
    {
        private JsonSerializerOptions Options { get; }

        public SystemTextJsonSerializer()
        {
            Options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            Options.Converters.Add(new ObjectToPrimitiveConverter());
            Options.Converters.Add(new FieldConverter());
        }

        public T Deserialize<T>(Stream stream)
        {
            return DeserializeAsync<T>(stream).Result;
        }

        public object Deserialize(Type type, Stream stream)
        {
            return DeserializeAsync(type, stream).Result;
        }

        public async Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, Options, cancellationToken);
        }

        public async Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync(stream, type, Options, cancellationToken);
        }

        public void Serialize<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.Indented)
        {
            JsonSerializer.Serialize(new Utf8JsonWriter(stream), data, Options);
        }

        public async Task SerializeAsync<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.Indented, CancellationToken cancellationToken = default)
        {
            await JsonSerializer.SerializeAsync<T>(stream, data, Options, cancellationToken);
        }
    }
}
