using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class EdgeByIdQuery : IRequest<Edge>
    {
        public Guid Id { get; }

        public EdgeByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
