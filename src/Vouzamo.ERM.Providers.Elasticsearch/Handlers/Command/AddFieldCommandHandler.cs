using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Converters;
using Vouzamo.ERM.CQRS;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class AddFieldCommandHandler<T> : IRequestHandler<AddFieldCommand<T>, T> where T : class, IHasFields, IIdentifiable<Guid>
    {
        protected IMediator Mediator { get; }
        protected IConverter Converter { get; }

        public AddFieldCommandHandler(IMediator mediator, IConverter converter)
        {
            Mediator = mediator;
            Converter = converter;
        }

        public async Task<T> Handle(AddFieldCommand<T> request, CancellationToken cancellationToken)
        {
            var types = await Mediator.Send(new ByIdQuery<T>(new List<Guid> { request.Id }));

            if(!types.TryGetValue(request.Id, out var type))
            {
                throw new KeyNotFoundException($"Node '{request.Id}' not found.");
            }

            var field = Converter.Convert<FieldDTO, Field>(request.Field);

            // Move into a generic List<T> extension? e.g. List<T>.Upsert(field)

            var existingField = type.Fields.FirstOrDefault(f => f.Key.Equals(field.Key));

            if (existingField != default)
            {
                type.Fields[type.Fields.IndexOf(existingField)] = field;
            }
            else
            {
                type.Fields.Add(field);
            }

            await Mediator.Send(new UpdateCommand<T>(type));

            return type;
        }
    }
}
