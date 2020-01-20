using GraphQL;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Api.Graph.Types.Fields;
using Vouzamo.ERM.Api.Graph.Types.Input;

namespace Vouzamo.ERM.Api.Graph
{
    public static class Extensions
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
            }).AddWebSockets();

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddSingleton<GuidGraphType>();

            services.AddSingleton<MySchema>();

            services.AddSingleton<MyQuery>();
            services.AddSingleton<MyMutation>();
            services.AddSingleton<MySubscription>();

            services.AddSingleton<JsonGraphType>();

            services.AddSingleton<NodeTypeGraphType>();
            services.AddSingleton<NodeGraphType>();
            services.AddSingleton<EdgeTypeGraphType>();
            services.AddSingleton<EdgeGraphType>();
            services.AddSingleton<FieldInterface>();
            services.AddSingleton<StringFieldGraphType>();
            services.AddSingleton<IntegerFieldGraphType>();

            services.AddSingleton<FieldInputType>();

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

        }
    }
}
