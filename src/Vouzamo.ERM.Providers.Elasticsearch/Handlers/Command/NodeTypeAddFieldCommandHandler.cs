using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Factories;
using Vouzamo.ERM.CQRS;
using Field = Vouzamo.ERM.Common.Field;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class NodeTypeAddFieldCommandHandler : IRequestHandler<NodeTypeAddFieldCommand, NodeType>
    {
        protected IMediator Mediator { get; }

        public NodeTypeAddFieldCommandHandler(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task<NodeType> Handle(NodeTypeAddFieldCommand request, CancellationToken cancellationToken)
        {
            var nodeTypes = await Mediator.Send(new NodeTypesByIdQuery(new List<Guid> { request.Id }));

            if(!nodeTypes.TryGetValue(request.Id, out var nodeType))
            {
                throw new KeyNotFoundException($"Node '{request.Id}' not found.");
            }

            Field field = FieldFactory.CreateField(request.Field);

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
