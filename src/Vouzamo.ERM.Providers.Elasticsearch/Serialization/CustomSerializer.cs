using Elasticsearch.Net;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common.Serialization;

namespace Vouzamo.ERM.Providers.Elasticsearch.Serialization
{
    public class CustomSerializer : IElasticsearchSerializer
    {
        protected IJsonSerializer Serializer { get; }

        public CustomSerializer(IJsonSerializer serializer)
        {
            Serializer = serializer;
        }

        public object Deserialize(Type type, Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();

                return Serializer.Deserialize(type, json);
            }
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();

                return Serializer.Deserialize<T>(json);
            }
        }

        public async Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();

                return await Serializer.DeserializeAsync(type, json, cancellationToken);
            }
        }

        public async Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();

                return await Serializer.DeserializeAsync<T>(json, cancellationToken);
            }
        }

        public void Serialize<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None)
        {
            var json = Serializer.Serialize(data);

            using (var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            {
                writer.Write(json);
            }
        }

        public async Task SerializeAsync<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None, CancellationToken cancellationToken = default)
        {
            var json = await Serializer.SerializeAsync(data);

            using (var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            {
                writer.Write(json);
            }
        }
    }
}
