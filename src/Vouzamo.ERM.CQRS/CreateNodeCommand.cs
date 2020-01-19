using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class CreateNodeCommand : IRequest<Node>
    {
        public Guid Id { get; }
        public Guid NodeType { get; }
        public string Name { get; }

        public CreateNodeCommand(Guid nodeType, string name, Guid? id = null)
        {
            NodeType = nodeType;
            Name = name;

            if (!id.HasValue)
            {
                id = Guid.NewGuid();
            }

            Id = id.Value;
        }
    }
}
