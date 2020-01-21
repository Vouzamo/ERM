using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class EdgeByIdQueryHandler : IRequestHandler<EdgeByIdQuery, Edge>
    {
        protected IElasticClient Client { get; }

        public EdgeByIdQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<Edge> Handle(EdgeByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.GetAsync<Edge>(request.Id, selector => selector.Index("edges"));

            if (!response.IsValid)
            {
                // todo: Wrap this in an application exception
                throw response.OriginalException;
            }

            return response.Source;
        }
    }
}
