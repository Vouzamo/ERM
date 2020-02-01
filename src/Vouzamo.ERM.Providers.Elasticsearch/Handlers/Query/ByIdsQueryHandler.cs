using MediatR;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Exceptions;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class ByIdsQueryHandler<T> : IRequestHandler<ByIdsQuery<T>, IDictionary<Guid, T>> where T : class, IIdentifiable<Guid>
    {
        protected IElasticClient Client { get; }

        public ByIdsQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<IDictionary<Guid, T>> Handle(ByIdsQuery<T> request, CancellationToken cancellationToken)
        {
            var ids = request.Ids.Select(id => id.ToString());

            var response = await Client.MultiGetAsync(m => m.GetMany<T>(ids), cancellationToken);

            var dictionary = response.SourceMany<T>(ids).ToDictionary(doc => doc.Id);

            return dictionary;
        }
    }
}
