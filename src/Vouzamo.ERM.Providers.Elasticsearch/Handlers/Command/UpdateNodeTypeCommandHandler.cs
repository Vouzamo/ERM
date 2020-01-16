using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class UpdateNodeTypeCommandHandler : IRequestHandler<UpdateNodeTypeCommand>
    {
        protected IElasticClient Client { get; }

        public UpdateNodeTypeCommandHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<Unit> Handle(UpdateNodeTypeCommand request, CancellationToken cancellationToken)
        {
            var response = await Client.UpdateAsync<NodeType>(request.NodeType.Id, descriptor => descriptor.Index("node-types").Doc(request.NodeType).DocAsUpsert(), cancellationToken);

            if (!response.IsValid)
            {
                throw response.OriginalException;
            }

            return new Unit();
        }
    }
}
