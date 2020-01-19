using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class NodesByNodeTypeQuery : IRequest<List<Node>>
    {
        public Guid NodeType { get; }
        public int Skip { get; }
        public int Take { get; }

        public NodesByNodeTypeQuery(Guid nodeType, int take, int skip = 0)
        {
            NodeType = nodeType;
            Take = Math.Max(1, take);
            Skip = Math.Max(0, skip);
        }
    }
}
