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
    public class NodesBySearchQueryHandler : IRequestHandler<NodesBySearchQuery, IEnumerable<Node>>
    {
        protected IElasticClient Client { get; }

        public NodesBySearchQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<IEnumerable<Node>> Handle(NodesBySearchQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.SearchAsync<Node>(descriptor => descriptor
                .Query(q => q
                    .MultiMatch(t => t
                        .Fields(fields => fields
                            .Field(field => field.Name)
                            .Field(field => field.Properties)
                        )
                        .Query(request.Query)
                    )
                )
                .Skip(request.Skip)
                .Size(request.Take)
                , cancellationToken
            );

            if (!response.IsValid)
            {
                throw response.OriginalException;
            }

            return response.Documents;
        }
    }
}
