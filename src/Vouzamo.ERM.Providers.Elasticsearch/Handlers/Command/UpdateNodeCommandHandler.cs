using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class UpdateNodeCommandHandler : IRequestHandler<UpdateNodeCommand>
    {
        protected IElasticClient Client { get; }

        public UpdateNodeCommandHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<Unit> Handle(UpdateNodeCommand request, CancellationToken cancellationToken)
        {
            var response = await Client.IndexAsync(request.Node, descriptor => descriptor, cancellationToken);

            if (!response.IsValid)
            {
                throw response.OriginalException;
            }

            return new Unit();
        }
    }
}
