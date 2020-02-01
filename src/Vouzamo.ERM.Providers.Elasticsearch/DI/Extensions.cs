using Elasticsearch.Net;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using Vouzamo.ERM.Common.Exceptions;
using Vouzamo.ERM.Common.Serialization;
using Vouzamo.ERM.CQRS;
using Vouzamo.ERM.CQRS.Command;
using Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command;
using Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query;
using Vouzamo.ERM.Providers.Elasticsearch.Serialization;

namespace Vouzamo.ERM.Providers.Elasticsearch.DI
{
    public static class Extensions
    {
        public static void AddElasticsearchProvider(this IServiceCollection services, ElasticsearchOptions options)
        {
            services.AddSingleton<IElasticClient>(serviceCollection =>
            {
                var connectionPool = new SingleNodeConnectionPool(options.Uri);

                var jsonSerializer = serviceCollection.GetService<IJsonSerializer>();

                var connectionSettings = new ConnectionSettings(connectionPool, (builtin, settings) => new CustomSerializer(jsonSerializer))
                .DefaultMappingFor<Common.Type>(mapping => mapping
                    .IndexName(options.TypesIndex)
                    .IdProperty(nodeType => nodeType.Id)
                )
                .DefaultMappingFor<Common.Node>(mapping => mapping
                    .IndexName(options.NodesIndex)
                    .IdProperty(node => node.Id)
                )
                .DefaultMappingFor<Common.Edge>(mapping => mapping
                    .IndexName(options.EdgesIndex)
                    .IdProperty(edge => edge.Id)
                )
                .OnRequestCompleted(request =>
                {
                    if(!request.Success)
                    {
                        throw CustomExceptions.StorageError(request.DebugInformation, request.OriginalException, request.OriginalException.Message);
                    }
                });

                var client = new ElasticClient(connectionSettings);

                ConfigureClient(client, options);

                return client;
            });

            services.AddMediatR(typeof(CreateEdgeCommandHandler));

            // Register any handlers that use unbound generics types
            services.AddSingleton<IRequestHandler<ByIdsQuery<Common.Type>, IDictionary<Guid, Common.Type>>, ByIdsQueryHandler<Common.Type>>();
            services.AddSingleton<IRequestHandler<ByIdsQuery<Common.Node>, IDictionary<Guid, Common.Node>>, ByIdsQueryHandler<Common.Node>>();
            services.AddSingleton<IRequestHandler<ByIdsQuery<Common.Edge>, IDictionary<Guid, Common.Edge>>, ByIdsQueryHandler<Common.Edge>>();
            services.AddSingleton<IRequestHandler<UpdateCommand<Common.Type>, Common.Type>, UpdateCommandHandler<Common.Type>>();
            services.AddSingleton<IRequestHandler<UpdateCommand<Common.Node>, Common.Node>, UpdateCommandHandler<Common.Node>>();
            services.AddSingleton<IRequestHandler<UpdateCommand<Common.Edge>, Common.Edge>, UpdateCommandHandler<Common.Edge>>();
            services.AddSingleton<IRequestHandler<RenameCommand<Common.Type>, bool>, RenameCommandHandler<Common.Type>>();
            services.AddSingleton<IRequestHandler<RenameCommand<Common.Node>, bool>, RenameCommandHandler<Common.Node>>();
            services.AddSingleton<IRequestHandler<AddFieldCommand<Common.Type>, bool>, AddFieldCommandHandler<Common.Type>>();
        }

        private static void ConfigureClient(IElasticClient client, ElasticsearchOptions options)
        {
            try
            {
                var typesExists = client.Indices.Exists(options.TypesIndex);

                if (!typesExists.Exists)
                {
                    var typesCreate = client.Indices.Create(options.TypesIndex, create => create
                        .Map<Common.Type>(m => m
                            .AutoMap()
                        )
                    );
                }

                var nodesExists = client.Indices.Exists(options.NodesIndex);

                if (!nodesExists.Exists)
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
            catch
            {
                // Suppress exceptions
            }
        }
    }
}
