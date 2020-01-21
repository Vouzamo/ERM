using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class NodeEdgesByNodesQuery : IRequest<ILookup<Guid, Edge>>
    {
        public IEnumerable<Guid> Nodes { get; }
        public Direction Direction { get; }

        public NodeEdgesByNodesQuery(IEnumerable<Guid> nodes, Direction direction)
        {
            Nodes = nodes;
            Direction = direction;
        }
    }
}
