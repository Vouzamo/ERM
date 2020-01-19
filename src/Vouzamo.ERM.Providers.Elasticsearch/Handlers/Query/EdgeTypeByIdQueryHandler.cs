using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class EdgeTypeByIdQueryHandler : IRequestHandler<EdgeTypeByIdQuery, EdgeType>
    {
        protected IElasticClient Client { get; }

        public EdgeTypeByIdQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<EdgeType> Handle(EdgeTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.GetAsync<EdgeType>(request.Id, selector => selector.Index("edge-types"));

            if (!response.IsValid)
            {
                // todo: Wrap this in an application exception
                throw response.OriginalException;
            }

            return response.Source;
        }
    }
}
