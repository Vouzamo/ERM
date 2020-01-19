using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class NodeByIdQueryHandler : IRequestHandler<NodeByIdQuery, Node>
    {
        protected IElasticClient Client { get; }

        public NodeByIdQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<Node> Handle(NodeByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.GetAsync<Node>(request.Id, selector => selector.Index("nodes"));

            if (!response.IsValid)
            {
                // todo: Wrap this in an application exception
                throw response.OriginalException;
            }

            return response.Source;
        }
    }
}
