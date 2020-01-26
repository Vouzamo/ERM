using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common.Serialization;

namespace Vouzamo.ERM.Api.Serialization
{
    public sealed class SystemTextJsonSerializer : IJsonSerializer
    {
        private JsonSerializerOptions Options { get; }

        public SystemTextJsonSerializer(JsonSerializerOptions options)
        {
            Options = options;
        }

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, Options);
        }

        public object Deserialize(Type type, string json)
        {
            return JsonSerializer.Deserialize(json, type, Options);
        }

        public Task<T> DeserializeAsync<T>(string json, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Deserialize<T>(json));
        }

        public Task<object> DeserializeAsync(Type type, string json, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Deserialize(type, json));
        }

        public string Serialize<T>(T data)
        {
            return JsonSerializer.Serialize(data, Options);
        }

        public Task<string> SerializeAsync<T>(T data, CancellationToken cancellationToken = default)
        {
            var json = Serialize(data);

            return Task.FromResult(json);
        }
    }
}
