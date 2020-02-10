using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class TypesBySearchQueryHandler : IRequestHandler<TypesBySearchQuery, BatchedResults<Type>>
    {
        protected IElasticClient Client { get; }

        public TypesBySearchQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<BatchedResults<Type>> Handle(TypesBySearchQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.SearchAsync<Type>(descriptor => descriptor
                .Query(q => {
                    var container = q.MultiMatch(t => t
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields(fields => fields
                            .Field(field => field.Name)
                        )
                        .Query(request.Query)
                    );

                    if (request.Scope.HasValue)
                    {
                        container &= q.Term(term => term
                            .Field(f => f.Scope)
                            .Value(request.Scope)
                        );
                    }

                    return container;
                })
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                , cancellationToken
            );

            if (!response.IsValid)
            {
                throw response.OriginalException;
            }

            return new BatchedResults<Type>(response.Documents, response.Total, request.Size, request.Page);
        }
    }
}
