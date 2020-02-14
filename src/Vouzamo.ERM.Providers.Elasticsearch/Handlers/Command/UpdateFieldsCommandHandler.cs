using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Converters;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class UpdateFieldsCommandHandler<T> : IRequestHandler<UpdateFieldsCommand<T>, T> where T : class, IHasFields, IIdentifiable<Guid>
    {
        protected IMediator Mediator { get; }
        protected IConverter Converter { get; }

        public UpdateFieldsCommandHandler(IMediator mediator, IConverter converter)
        {
            Mediator = mediator;
            Converter = converter;
        }

        public async Task<T> Handle(UpdateFieldsCommand<T> request, CancellationToken cancellationToken)
        {
            var type = await Mediator.Send(new ByIdQuery<T>(request.Id));

            type.Fields = request.Fields;

            var updated = await Mediator.Send(new UpdateCommand<T>(type));

            return updated;
        }
    }
}
