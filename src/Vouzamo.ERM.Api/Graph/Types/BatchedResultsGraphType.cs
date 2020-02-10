using GraphQL.Types;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class BatchedResultsGraphType<T, TGraph> : ObjectGraphType<BatchedResults<T>> where TGraph : ObjectGraphType<T>
    {
        public BatchedResultsGraphType()
        {
            Field<ListGraphType<TGraph>>("results", resolve: context => context.Source.Results);
            Field(field => field.TotalCount);
            Field(field => field.Page);
            Field(field => field.Size);

            Field<BooleanGraphType>("hasPrevious", resolve: context => context.Source.Page > 1);
            Field<BooleanGraphType>("hasNext", resolve: context => context.Source.TotalCount > (context.Source.Page * context.Source.Size));
        }
    }
}
