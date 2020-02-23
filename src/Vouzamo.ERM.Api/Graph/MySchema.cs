using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using System;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Api.Graph.Types.Fields;

namespace Vouzamo.ERM.Api.Graph
{
    public class MySchema : Schema
    {
        public MySchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<MyQuery>();
            Mutation = serviceProvider.GetRequiredService<MyMutation>();
            Subscription = serviceProvider.GetRequiredService<MySubscription>();

            RegisterType<StringFieldGraphType>();
            RegisterType<IntegerFieldGraphType>();
            RegisterType<NestedFieldGraphType>();

            RegisterValueConverter(new JsonGraphTypeConverter());
        }
    }
}
