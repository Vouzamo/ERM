using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Api.Graph.Types.Fields;
using Vouzamo.ERM.Api.Graph.Types.Input;

namespace Vouzamo.ERM.Api.Graph
{
    public class MySchema : Schema
    {
        public MySchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<MyQuery>();
            Mutation = resolver.Resolve<MyMutation>();
            Subscription = resolver.Resolve<MySubscription>();

            RegisterType<StringFieldGraphType>();
            RegisterType<IntegerFieldGraphType>();

            RegisterValueConverter(new JsonGraphTypeConverter());
        }
    }
}
