using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class NodesBySearchQueryHandler : IRequestHandler<NodesBySearchQuery, BatchedResults<Node>>
    {
        protected IElasticClient Client { get; }

        public NodesBySearchQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<BatchedResults<Node>> Handle(NodesBySearchQuery request, CancellationToken cancellationToken)
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
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                , cancellationToken
            );

            if (!response.IsValid)
            {
                throw response.OriginalException;
            }

            return new BatchedResults<Node>(response.Documents, response.Total, request.Size, request.Page);
        }
    }
}
