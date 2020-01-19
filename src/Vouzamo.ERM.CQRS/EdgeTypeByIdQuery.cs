using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class EdgeTypeByIdQuery : IRequest<EdgeType>
    {
        public Guid Id { get; }

        public EdgeTypeByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
