using Elasticsearch.Net;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
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
                .DefaultMappingFor<Common.Node>(mapping => mapping
                    .IndexName(options.NodesIndex)
                    .IdProperty(node => node.Id)
                )
                .DefaultMappingFor<Common.NodeType>(mapping => mapping
                    .IndexName(options.NodeTypesIndex)
                    .IdProperty(nodeType => nodeType.Id)
                )
                .DefaultMappingFor<Common.Edge>(mapping => mapping
                    .IndexName(options.EdgesIndex)
                    .IdProperty(edge => edge.Id)
                )
                .DefaultMappingFor<Common.EdgeType>(mapping => mapping
                    .IndexName(options.EdgeTypesIndex)
                    .IdProperty(edgeType => edgeType.Id)
                );

                var client = new ElasticClient(connectionSettings);

                ConfigureClient(client, options);

                return client;
            });

            services.AddMediatR(typeof(CreateNodeTypeCommandHandler));
        }

        private static void ConfigureClient(IElasticClient client, ElasticsearchOptions options)
        {
            var nodesExists = client.Indices.Exists(options.NodesIndex);

            if(!nodesExists.Exists)
            {
                var nodesCreate = client.Indices.Create(options.NodesIndex, create => create
                    .Map<Common.Node>(m => m
                        .AutoMap()
                        .Properties(properties => properties
                            .Flattened(flattened => flattened
                                .Name(node => node.Properties)
                            )
                        )
                    )
                );
            }

            var edgesExists = client.Indices.Exists(options.EdgesIndex);

            if (!edgesExists.Exists)
            {
                var edgesCreate = client.Indices.Create(options.EdgesIndex, create => create
                    .Map<Common.Edge>(m => m
                        .AutoMap()
                        .Properties(properties => properties
                            .Flattened(flattened => flattened
                                .Name(edge => edge.Properties)
                            )
                        )
                    )
                );
            }
        }
    }
}
