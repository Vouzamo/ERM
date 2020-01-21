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
    public class EdgeTypesByIdQueryHandler : IRequestHandler<EdgeTypesByIdQuery, IDictionary<Guid, EdgeType>>
    {
        protected IElasticClient Client { get; }

        public EdgeTypesByIdQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<IDictionary<Guid, EdgeType>> Handle(EdgeTypesByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.SearchAsync<EdgeType>(descriptor => descriptor
                .Index("edge-types")
                .Query(q => q
                    .Terms(t => t
                        .Field(f => f.Id.Suffix("keyword"))
                        .Terms(request.Ids)
                    )
                )
                .Size(10000),
                cancellationToken
            );

            if (!response.IsValid)
            {
                // todo: Wrap this in an application exception
                throw response.OriginalException;
            }

            return response.Documents.ToDictionary(doc => doc.Id);
        }
    }
}
