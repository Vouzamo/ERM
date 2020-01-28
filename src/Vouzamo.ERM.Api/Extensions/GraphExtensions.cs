﻿using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Vouzamo.ERM.Api.Graph;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Api.Graph.Types.Fields;
using Vouzamo.ERM.Api.Graph.Types.Input;

namespace Vouzamo.ERM.Api.Extensions
{
    public static class GraphExtensions
    {
        public static void AddGraph(this IServiceCollection services)
        {
            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            services.AddHttpContextAccessor();

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = true;
            }).AddWebSockets()
              .AddDataLoader()
              .AddGraphTypes();

            services.AddSingleton<MySchema>();

            services.AddSingleton<MyQuery>();
            services.AddSingleton<MyMutation>();
            services.AddSingleton<MySubscription>();

            services.AddSingleton<JsonGraphType>();

            services.AddSingleton<TypeGraphType>();
            services.AddSingleton<NodeGraphType>();
            services.AddSingleton<EdgeGraphType>();

            services.AddSingleton<EdgeTraversalGraphType>();

            services.AddSingleton<FieldInterface>();
            services.AddSingleton<StringFieldGraphType>();
            services.AddSingleton<IntegerFieldGraphType>();

            services.AddSingleton<FieldInputType>();

            services.AddSingleton<TypeScopeEnumerationGraphType>();
            services.AddSingleton<DirectionEnumerationGraphType>();
        }

        public static void UseGraph(this IApplicationBuilder app)
        {
            app.UseWebSockets();

            // Enable endpoint for websockets (subscriptions)
            app.UseGraphQLWebSockets<MySchema>("/graphql");
            // Enable endpoint for querying
            app.UseGraphQL<MySchema>("/graphql");

            app.UseGraphiQLServer(new GraphiQLOptions());

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
        }
    }
}