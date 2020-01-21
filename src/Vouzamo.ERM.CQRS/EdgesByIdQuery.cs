using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class EdgesByIdQuery : IRequest<IDictionary<Guid, Edge>>
    {
        public IEnumerable<Guid> Ids { get; }

        public EdgesByIdQuery(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}
