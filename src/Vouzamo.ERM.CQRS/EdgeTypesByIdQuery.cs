using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class EdgeTypesByIdQuery : IRequest<IDictionary<Guid, EdgeType>>
    {
        public IEnumerable<Guid> Ids { get; }

        public EdgeTypesByIdQuery(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}
