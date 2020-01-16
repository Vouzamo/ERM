using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class NodeTypeByIdQuery : IRequest<NodeType>
    {
        public Guid Id { get; }

        public NodeTypeByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
