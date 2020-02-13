using GraphQL;
using GraphQL.Types;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Api.Graph.Types.Fields;

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
            RegisterType<NestedFieldGraphType>();

            RegisterValueConverter(new JsonGraphTypeConverter());
        }
    }
}
