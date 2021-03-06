﻿using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class CreateNodeCommandHandler : IRequestHandler<CreateNodeCommand, Node>
    {
        protected IElasticClient Client { get; }

        public CreateNodeCommandHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<Node> Handle(CreateNodeCommand request, CancellationToken cancellationToken)
        {
            var document = new Node(request.Id, request.Type, request.Name);

            var response = await Client.CreateDocumentAsync(document, cancellationToken);

            if (!response.IsValid)
            {
                // todo: Wrap this in an application exception
                throw response.OriginalException;
            }

            return document;
        }
    }
}
