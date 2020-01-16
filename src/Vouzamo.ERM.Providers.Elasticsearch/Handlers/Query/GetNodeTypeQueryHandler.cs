using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class GetNodeTypeQueryHandler : IRequestHandler<NodeTypeByIdQuery, NodeType>
    {
        protected IElasticClient Client { get; }

        public GetNodeTypeQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<NodeType> Handle(NodeTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.GetAsync<NodeType>(request.Id, selector => selector.Index("node-types"));

            if(!response.IsValid)
            {
                // todo: Wrap this in an application exception
                throw response.OriginalException;
            }

            return response.Source;
        }
    }
}
