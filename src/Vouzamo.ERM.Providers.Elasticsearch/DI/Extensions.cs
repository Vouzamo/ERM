﻿using Elasticsearch.Net;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using Vouzamo.ERM.Providers.Elasticsearch.Handlers;
using Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command;

namespace Vouzamo.ERM.Providers.Elasticsearch.DI
{
    public static class Extensions
    {
        public static void AddElasticsearchProvider(this IServiceCollection services, ElasticsearchOptions options)
        {
            services.AddTransient<IElasticClient>(serviceCollection =>
            {
                var connectionPool = new SingleNodeConnectionPool(options.Uri);

                var connectionSettings = new ConnectionSettings(connectionPool)
                    .DisableDirectStreaming()
                    .PrettyJson()
                    .DefaultIndex("nodes");

                return new ElasticClient(connectionSettings);
            });

            services.AddMediatR(typeof(CreateNodeTypeCommandHandler));
        }
    }
}