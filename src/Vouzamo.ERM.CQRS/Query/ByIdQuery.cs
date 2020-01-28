using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class ByIdQuery<T> : IRequest<IDictionary<Guid, T>> where T : IIdentifiable<Guid>
    {
        public IEnumerable<Guid> Ids { get; }

        public ByIdQuery(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}
