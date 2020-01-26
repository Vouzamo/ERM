using MediatR;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class CreateEdgeCommandHandler : IRequestHandler<CreateEdgeCommand, Edge>
    {
        protected IElasticClient Client { get; }

        public CreateEdgeCommandHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<Edge> Handle(CreateEdgeCommand request, CancellationToken cancellationToken)
        {
            var document = new Edge(request.Id, request.Type, request.From, request.To);

            var typeExists = await Client.DocumentExistsAsync<EdgeType>(request.Type, selector => selector, cancellationToken);
            var fromExists = await Client.DocumentExistsAsync<Node>(request.From, selector => selector, cancellationToken);
            var toExists = await Client.DocumentExistsAsync<Node>(request.To, selector => selector, cancellationToken);

            if(!typeExists.Exists || !fromExists.Exists || !toExists.Exists)
            {
                throw new DataMisalignedException("One or more of the ids does not exist");
            }

            var response = await Client.IndexAsync(document, descriptor => descriptor, cancellationToken);

            if (!response.IsValid)
            {
                // todo: Wrap this in an application exception
                throw response.OriginalException;
            }

            return document;
        }
    }
}
