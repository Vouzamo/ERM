using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vouzamo.ERM.Api.Graph
{
    public class MySchema : Schema
    {
        public MySchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<MyQuery>();
            Mutation = resolver.Resolve<MyMutation>();
            Subscription = resolver.Resolve<MySubscription>();
        }
    }
}
