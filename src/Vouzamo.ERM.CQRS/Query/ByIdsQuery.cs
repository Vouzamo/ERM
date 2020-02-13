using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class ByIdsQuery<T> : IRequest<IDictionary<Guid, T>> where T : IIdentifiable<Guid>
    {
        public IEnumerable<Guid> Ids { get; }

        public ByIdsQuery(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}
