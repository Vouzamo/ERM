using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Internal;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Vouzamo.ERM.Api.Graph;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Api.Graph.Types.Fields;
using Vouzamo.ERM.Api.Graph.Types.Input;
using Vouzamo.ERM.Api.Handlers;
using Vouzamo.ERM.Common.Exceptions;
using Vouzamo.ERM.CQRS;

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
                options.EnableMetrics = false;
                options.ExposeExceptions = false;
                options.SetFieldMiddleware = true;
            })
            .AddGraphTypes()
            .AddDataLoader()
            .AddWebSockets();

            services.AddSingleton(typeof(IGraphQLExecuter<>), typeof(MyGraphQLExecutor<>));

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

            services.AddSingleton<NotificationMessageGraphType>();

            // Mediatr
            services.AddSingleton<IRequestHandler<ByIdQuery<Common.Type>, Common.Type>, ByIdQueryHandler<Common.Type>>();
            services.AddSingleton<IRequestHandler<ByIdQuery<Common.Node>, Common.Node>, ByIdQueryHandler<Common.Node>>();
            services.AddSingleton<IRequestHandler<ByIdQuery<Common.Edge>, Common.Edge>, ByIdQueryHandler<Common.Edge>>();
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

        public static void AddException(this ResolveFieldContext context, Exception exception)
        {
            switch(exception)
            {
                case AggregateException aggregateException:
                    foreach(var innerException in aggregateException.InnerExceptions)
                    {
                        context.AddException(innerException);
                    }
                    break;
                case CustomException customException:
                    var error = new ExecutionError(customException.Message, customException)
                    {
                        Code = customException.Code,
                        Path = context.Path
                    };

                    error.DataAsDictionary.Add("debug", customException.Debug);

                    context.Errors.Add(error);
                    break;
                default:
                    context.AddException(CustomExceptions.UnknownError(exception));
                    break;
            }
        }
    }
}
