using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class NodeByIdQuery : IRequest<Node>
    {
        public Guid Id { get; }

        public NodeByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
