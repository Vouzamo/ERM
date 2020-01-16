using MediatR;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class UpdateNodeTypeCommand : IRequest
    {
        public NodeType NodeType { get; }

        public UpdateNodeTypeCommand(NodeType nodeType)
        {
            NodeType = nodeType;
        }
    }
}
