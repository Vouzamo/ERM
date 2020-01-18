using GraphQL.Types;

namespace Vouzamo.ERM.Api.Graph
{
    public class MyMutation : ObjectGraphType
    {
        public MyMutation()
        {
            Name = "Mutation";

            Field<StringGraphType>("temp", resolve: (context) => string.Empty);
        }
    }
}
