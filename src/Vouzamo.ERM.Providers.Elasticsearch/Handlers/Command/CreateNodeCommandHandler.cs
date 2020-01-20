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
            var document = new Node(request.Id, request.NodeType, request.Name);

            var response = await Client.IndexAsync(document, descriptor => descriptor
                .Index("nodes")
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