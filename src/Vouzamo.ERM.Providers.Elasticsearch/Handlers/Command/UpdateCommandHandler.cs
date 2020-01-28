using MediatR;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class UpdateCommandHandler<T> : IRequestHandler<UpdateCommand<T>, T> where T : class, IIdentifiable<Guid>
    {
        protected IElasticClient Client { get; }

        public UpdateCommandHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<T> Handle(UpdateCommand<T> request, CancellationToken cancellationToken)
        {
            var response = await Client.UpdateAsync<T>(request.Updated.Id, descriptor => descriptor.Doc(request.Updated), cancellationToken);

            if (!response.IsValid)
            {
                throw response.OriginalException;
            }

            return request.Updated;
        }
    }
}
