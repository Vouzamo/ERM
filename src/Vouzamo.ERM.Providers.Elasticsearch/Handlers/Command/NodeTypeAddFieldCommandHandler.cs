using MediatR;
using System;
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
            var nodeType = await Mediator.Send(new NodeTypeByIdQuery(request.Id));

            Field field = FieldFactory.CreateField(request.Field);

            if (nodeType.Fields.Any(f => f.Key.Equals(field.Key)))
            {
                throw new NotSupportedException("field key must be unique within collection.");
            }

            nodeType.Fields.Add(field);

            await Mediator.Send(new UpdateNodeTypeCommand(nodeType));

            return nodeType;
        }
    }
}
