using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class NodesByTypesQuery : IRequest<ILookup<Guid, Node>>
    {
        public IEnumerable<Guid> NodeTypes { get; }
        public int Skip { get; }
        public int Take { get; }

        public NodesByTypesQuery(IEnumerable<Guid> types, int take, int skip = 0)
        {
            NodeTypes = types;
            Take = Math.Max(1, take);
            Skip = Math.Max(0, skip);
        }
    }
}
