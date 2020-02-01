using GraphQL;
using GraphQL.Execution;
using GraphQL.Server;
using GraphQL.Server.Internal;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Api.Extensions;
using Vouzamo.ERM.Common.Exceptions;

namespace Vouzamo.ERM.Api.Graph
{
    public class MyGraphQLExecutor<TSchema> : DefaultGraphQLExecuter<TSchema> where TSchema : ISchema
    {
        public MyGraphQLExecutor(TSchema schema, IDocumentExecuter documentExecuter, IOptions<GraphQLOptions> options, IEnumerable<IDocumentExecutionListener> listeners, IEnumerable<IValidationRule> validationRules) : base(schema, documentExecuter, options, listeners, validationRules)
        {
            
        } 

        public override async Task<ExecutionResult> ExecuteAsync(string operationName, string query, Inputs variables, object context, CancellationToken cancellationToken = default)
        {
            var result = await base.ExecuteAsync(operationName, query, variables, context, cancellationToken);

            // enrichment

            return result;
        }

        protected override ExecutionOptions GetOptions(string operationName, string query, Inputs variables, object context, CancellationToken cancellationToken)
        {
            var options = base.GetOptions(operationName, query, variables, context, cancellationToken);

            if (!options.ExposeExceptions)
            {
                // Box Exceptions
                options.FieldMiddleware.Use(next => {
                    return async context =>
                    {
                        try
                        {
                            return await next(context);
                        }
                        catch (Exception exception)
                        {
                            context.AddException(exception);
                        }

                        return null;
                    };
                });
            }

            return options;
        }
    }
}
