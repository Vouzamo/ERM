using MediatR;
using Nest;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class CreateNodeTypeCommandHandler : IRequestHandler<CreateNodeTypeCommand, NodeType>
    {
        protected IElasticClient Client { get; }

        public CreateNodeTypeCommandHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<NodeType> Handle(CreateNodeTypeCommand request, CancellationToken cancellationToken)
        {
            var document = new NodeType(request.Id, request.Name);

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
