using GraphQL;
using GraphQL.Authorization;
using GraphQL.Server;
using GraphQL.Server.Internal;
using GraphQL.Server.Ui.Playground;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using GraphQL.Validation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Text.Json;
using Vouzamo.ERM.Api.Graph;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Api.Graph.Types.Fields;
using Vouzamo.ERM.Api.Handlers;
using Vouzamo.ERM.Common.Exceptions;
using Vouzamo.ERM.Common.Serialization;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Extensions
{
    public static class GraphExtensions
    {
        public static void AddGraphQLAuth(this IServiceCollection services, Action<AuthorizationSettings> configure)
        {
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            services.TryAddTransient(s =>
            {
                var authSettings = new AuthorizationSettings();

                configure(authSettings);

                return authSettings;
            });
        }

        public static void AddGraph(this IServiceCollection services)
        {
            //services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            //services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddHttpContextAccessor();

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddGraphQLAuth(settings =>
            {
                settings.AddPolicy("Command", p => p.RequireClaim("cognito:groups", "Command"));
                settings.AddPolicy("Query", p => p.RequireClaim("cognito:groups", "Query"));
            });

            services.AddGraphQL(options =>
            {
                options.EnableMetrics = false;
                options.ExposeExceptions = false;
            })
            .AddSystemTextJson(
                options => {
                    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.Converters.Add(new ObjectToPrimitiveConverter());
                    options.Converters.Add(new FieldConverter());
                }, options => {
                    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.Converters.Add(new ObjectToPrimitiveConverter());
                    options.Converters.Add(new FieldConverter());
                }
             )
            .AddGraphTypes()
            .AddDataLoader()
            .AddWebSockets()
            .AddUserContextBuilder(context => new GraphQLUserContext { User = context.User });

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
            services.AddSingleton<NestedFieldGraphType>();

            services.AddSingleton<TypeScopeEnumerationGraphType>();
            services.AddSingleton<DirectionEnumerationGraphType>();
            services.AddSingleton<SeverityEnumerationGraphType>();

            services.AddSingleton<NotificationMessageGraphType>();

            // Mediatr
            services.AddSingleton<IRequestHandler<ByIdQuery<Common.Type>, Common.Type>, ByIdQueryHandler<Common.Type>>();
            services.AddSingleton<IRequestHandler<ByIdQuery<Common.Node>, Common.Node>, ByIdQueryHandler<Common.Node>>();
            services.AddSingleton<IRequestHandler<ByIdQuery<Common.Edge>, Common.Edge>, ByIdQueryHandler<Common.Edge>>();
        }

        public static void UseGraph(this IApplicationBuilder app)
        {
            app.UseWebSockets();

            // Enable endpoint for querying
            app.UseGraphQL<MySchema>("/graphql");

            // Enable endpoint for websockets (subscriptions)
            app.UseGraphQLWebSockets<MySchema>("/graphql");

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
        }

        public static void AddException(this IResolveFieldContext context, Exception exception)
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
