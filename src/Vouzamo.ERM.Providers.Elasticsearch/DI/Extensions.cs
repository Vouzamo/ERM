using Elasticsearch.Net;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System.Text;
using Vouzamo.ERM.Common.Serialization;
using Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command;
using Vouzamo.ERM.Providers.Elasticsearch.Serialization;

namespace Vouzamo.ERM.Providers.Elasticsearch.DI
{
    public static class Extensions
    {
        public static void AddElasticsearchProvider(this IServiceCollection services, ElasticsearchOptions options)
        {
            services.AddTransient<IElasticClient>(serviceCollection =>
            {
                var connectionPool = new SingleNodeConnectionPool(options.Uri);

                var jsonSerializer = serviceCollection.GetService<IJsonSerializer>();

                var connectionSettings = new ConnectionSettings(connectionPool, (builtin, settings) => new CustomSerializer(jsonSerializer))
                    .DisableDirectStreaming()
                    .PrettyJson()
                    .OnRequestCompleted(handler =>
                    {
                        var request = Encoding.UTF8.GetString(handler.RequestBodyInBytes);
                        var response = Encoding.UTF8.GetString(handler.ResponseBodyInBytes);
                    });


                return new ElasticClient(connectionSettings);
            });

            services.AddMediatR(typeof(CreateNodeTypeCommandHandler));
        }
    }
}
