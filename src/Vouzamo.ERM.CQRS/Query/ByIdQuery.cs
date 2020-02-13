using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class ByIdQuery<T> : IRequest<T> where T : IIdentifiable<Guid>
    {
        public Guid Id { get; }

        public ByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
