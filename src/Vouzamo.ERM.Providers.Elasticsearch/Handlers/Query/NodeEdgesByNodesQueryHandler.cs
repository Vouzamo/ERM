using MediatR;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class NodeEdgesByNodesQueryHandler : IRequestHandler<NodeEdgesByNodesQuery, ILookup<Guid, Edge>>
    {
        protected IElasticClient Client { get; }

        public NodeEdgesByNodesQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<ILookup<Guid, Edge>> Handle(NodeEdgesByNodesQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.SearchAsync<Edge>(descriptor => descriptor
                .Query(q => q
                    .Terms(t => t
                        .Field(request.Direction.Equals(Direction.Outbound) ? "from.keyword" : "to.keyword")
                        .Terms(request.Nodes)
                    )
                ),
                cancellationToken
            );

            if (!response.IsValid)
            {
                throw response.OriginalException;
            }

            return request.Direction.Equals(Direction.Outbound) ? response.Documents.ToLookup(node => node.From) : response.Documents.ToLookup(node => node.To);
        }
    }
}
