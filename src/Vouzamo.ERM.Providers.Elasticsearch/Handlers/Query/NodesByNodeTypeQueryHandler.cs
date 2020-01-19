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
    public class NodesByNodeTypeQueryHandler : IRequestHandler<NodesByNodeTypeQuery, List<Node>>
    {
        protected IElasticClient Client { get; }

        public NodesByNodeTypeQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<List<Node>> Handle(NodesByNodeTypeQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.SearchAsync<Node>(descriptor => descriptor
                .Index("nodes")
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.Type.Suffix("keyword"))
                        .Value(request.NodeType)
                    )
                )
                .Skip(request.Skip)
                .Size(request.Take)
            );

            if (!response.IsValid)
            {
                throw response.OriginalException;
            }

            return response.Documents.ToList();
        }
    }
}
