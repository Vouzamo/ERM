using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Vouzamo.ERM.Common.Serialization
{
    public interface IJsonSerializer
    {
        T Deserialize<T>(string json);
        object Deserialize(Type type, string json);
        Task<T> DeserializeAsync<T>(string json, CancellationToken cancellationToken = default);
        Task<object> DeserializeAsync(Type type, string json, CancellationToken cancellationToken = default);

        string Serialize<T>(T data);
        Task<string> SerializeAsync<T>(T data, CancellationToken cancellationToken = default);
    }
}
