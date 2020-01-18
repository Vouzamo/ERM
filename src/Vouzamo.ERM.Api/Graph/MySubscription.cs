using GraphQL.Types;

namespace Vouzamo.ERM.Api.Graph
{
    public class MySubscription : ObjectGraphType
    {
        public MySubscription()
        {
            Name = "Mutation";

            Field<StringGraphType>("temp", resolve: (context) => string.Empty);
        }
    }
}
