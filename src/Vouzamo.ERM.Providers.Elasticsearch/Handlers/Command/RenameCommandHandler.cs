using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS.Command;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class RenameCommandHandler<T> : IRequestHandler<RenameCommand<T>, bool> where T : class, IHasName
    {
        protected IElasticClient Client { get; set; }

        public RenameCommandHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<bool> Handle(RenameCommand<T> request, CancellationToken cancellationToken)
        {
            var response = await Client.UpdateAsync<T, RenameCommand<T>>(request.Id, descriptor => descriptor.DocAsUpsert().Doc(request));

            if(!response.IsValid)
            {
                throw response.OriginalException;
            }

            return response.IsValid;
        }
    }
}
