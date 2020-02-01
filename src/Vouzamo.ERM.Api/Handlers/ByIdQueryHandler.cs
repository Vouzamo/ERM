using GraphQL.DataLoader;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Exceptions;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Api.Handlers
{
    public class ByIdQueryHandler<T> : IRequestHandler<ByIdQuery<T>, T> where T : IIdentifiable<Guid>
    {
        protected IMediator Mediator { get; }
        protected IDataLoaderContextAccessor Accessor { get; }

        public ByIdQueryHandler(IMediator mediator, IDataLoaderContextAccessor accessor)
        {
            Mediator = mediator;
            Accessor = accessor;
        }

        public async Task<T> Handle(ByIdQuery<T> request, CancellationToken cancellationToken)
        {
            var key = $"Get{typeof(T).Name}ById";

            var loader = Accessor.Context.GetOrAddBatchLoader<Guid, T>(key, async (ids, cancellationToken) =>
            {
                return await Mediator.Send(new ByIdsQuery<T>(ids));
            });

            var data = await loader.LoadAsync(request.Id);

            if (data == null)
            {
                throw CustomExceptions.GuidNotFoundError(request.Id, typeof(T).Name);
            }

            return data;
        }
    }
}
