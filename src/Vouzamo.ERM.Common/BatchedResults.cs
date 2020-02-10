using System.Collections.Generic;
using System.Linq;

namespace Vouzamo.ERM.Common
{
    public class BatchedResults<T>
    {
        public IEnumerable<T> Results { get; }
        public long TotalCount { get; }
        public int Size { get; }
        public int Page { get; }

        protected BatchedResults()
        {
            Results = Enumerable.Empty<T>();
            TotalCount = 0;
            Size = 0;
            Page = 1;
        }

        public BatchedResults(int size, int page = 1) : this()
        {
            Size = size;
            Page = page;
        }

        public BatchedResults(IEnumerable<T> results, long totalCount, int size, int page = 1) : this(size, page)
        {
            Results = results;
            TotalCount = totalCount;
        }
    }
}
