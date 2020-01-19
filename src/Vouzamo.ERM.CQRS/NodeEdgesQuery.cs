using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class NodeEdgesQuery : IRequest<List<Edge>>
    {
        public Guid Node { get; }
        public Direction Direction { get; }

        public NodeEdgesQuery(Guid node, Direction direction)
        {
            Node = node;
            Direction = direction;
        }
    }
}
