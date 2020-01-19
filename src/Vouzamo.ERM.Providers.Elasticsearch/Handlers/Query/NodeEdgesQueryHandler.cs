using MediatR;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class NodeEdgesQueryHandler : IRequestHandler<NodeEdgesQuery, List<Edge>>
    {
        protected IElasticClient Client { get; }

        public NodeEdgesQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<List<Edge>> Handle(NodeEdgesQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.SearchAsync<Edge>(descriptor => descriptor
                .Index("edges")
                .Query(q => q
                    .Term(t => t
                        .Field(request.Direction.Equals(Direction.Outbound) ? "from.keyword" : "to.keyword")
                        .Value(request.Node)
                    )
                )
            );

            if (!response.IsValid)
            {
                throw response.OriginalException;
            }

            return response.Documents.ToList();
        }
    }
}
