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
    public class NodeTypeAddFieldCommandHandler : IRequestHandler<NodeTypeAddFieldCommand, NodeType>
    {
        protected IMediator Mediator { get; }
        protected IConverter Converter { get; }

        public NodeTypeAddFieldCommandHandler(IMediator mediator, IConverter converter)
        {
            Mediator = mediator;
            Converter = converter;
        }

        public async Task<NodeType> Handle(NodeTypeAddFieldCommand request, CancellationToken cancellationToken)
        {
            var nodeTypes = await Mediator.Send(new NodeTypesByIdQuery(new List<Guid> { request.Id }));

            if(!nodeTypes.TryGetValue(request.Id, out var nodeType))
            {
                throw new KeyNotFoundException($"Node '{request.Id}' not found.");
            }

            var field = Converter.Convert<FieldDTO, Field>(request.Field);

            // Move into a generic List<T> extension? e.g. List<T>.Upsert(field)

            var existingField = nodeType.Fields.FirstOrDefault(f => f.Key.Equals(field.Key));

            if (existingField != default)
            {
                nodeType.Fields[nodeType.Fields.IndexOf(existingField)] = field;
            }
            else
            {
                nodeType.Fields.Add(field);
            }

            await Mediator.Send(new UpdateNodeTypeCommand(nodeType));

            return nodeType;
        }
    }
}
