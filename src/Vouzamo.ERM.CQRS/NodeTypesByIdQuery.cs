using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class NodeTypesByIdQuery : IRequest<IDictionary<Guid, NodeType>>
    {
        public IEnumerable<Guid> Ids { get; }

        public NodeTypesByIdQuery(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}
