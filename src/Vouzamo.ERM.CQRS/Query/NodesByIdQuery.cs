using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class NodesByIdQuery : IRequest<IDictionary<Guid, Node>>
    {
        public IEnumerable<Guid> Ids { get; }

        public NodesByIdQuery(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}
