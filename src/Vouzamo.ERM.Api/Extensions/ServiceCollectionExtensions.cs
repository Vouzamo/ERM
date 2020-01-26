using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Vouzamo.ERM.Api.Serialization;
using Vouzamo.ERM.Common.Serialization;

namespace Vouzamo.ERM.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSystemTextJsonSerializer(this IServiceCollection services)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            options.Converters.Add(new ObjectToPrimitiveConverter());
            options.Converters.Add(new FieldConverter());

            services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>((serviceProvider) => new SystemTextJsonSerializer(options));
        }
    }
}
