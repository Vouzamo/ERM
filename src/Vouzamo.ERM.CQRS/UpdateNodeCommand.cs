using MediatR;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class UpdateNodeCommand : IRequest
    {
        public Node Node { get; }

        public UpdateNodeCommand(Node node)
        {
            Node = node;
        }
    }
}
