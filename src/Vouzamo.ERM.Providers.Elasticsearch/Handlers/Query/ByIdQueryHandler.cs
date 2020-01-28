using MediatR;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class ByIdQueryHandler<T> : IRequestHandler<ByIdQuery<T>, IDictionary<Guid, T>> where T : class, IIdentifiable<Guid>
    {
        protected IElasticClient Client { get; }

        public ByIdQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<IDictionary<Guid, T>> Handle(ByIdQuery<T> request, CancellationToken cancellationToken)
        {
            var ids = request.Ids.Select(id => id.ToString());

            var response = await Client.MultiGetAsync(m => m.GetMany<T>(ids), cancellationToken);

            if (!response.IsValid)
            {
                // todo: Wrap this in an application exception
                throw response.OriginalException;
            }

            return response.SourceMany<T>(ids).ToDictionary(doc => doc.Id);
        }
    }
}
