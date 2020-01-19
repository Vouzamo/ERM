using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class CreateEdgeTypeCommandHandler : IRequestHandler<CreateEdgeTypeCommand, EdgeType>
    {
        protected IElasticClient Client { get; }

        public CreateEdgeTypeCommandHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<EdgeType> Handle(CreateEdgeTypeCommand request, CancellationToken cancellationToken)
        {
            var document = new EdgeType(request.Id, request.Name);

            var response = await Client.IndexAsync(document, descriptor => descriptor
                .Index("edge-types")
                .Id(document.Id)
            , cancellationToken);

            if (!response.IsValid)
            {
                // todo: Wrap this in an application exception
                throw response.OriginalException;
            }

            return document;
        }
    }
}
