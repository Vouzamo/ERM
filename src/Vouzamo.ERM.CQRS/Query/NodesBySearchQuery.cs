using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class NodesBySearchQuery : IRequest<IEnumerable<Node>>
    {
        public string Query { get; }
        public int Take { get; }
        public int Skip { get; }

        public NodesBySearchQuery(string query, int take, int skip = 0)
        {
            Query = query;
            Take = Math.Max(1, take);
            Skip = Math.Max(0, skip);
        }
    }
}
