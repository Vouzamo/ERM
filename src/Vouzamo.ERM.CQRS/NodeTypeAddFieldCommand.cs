using MediatR;
using System;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.CQRS
{
    public class NodeTypeAddFieldCommand : IRequest<NodeType>
    {
        public Guid Id { get; }
        public FieldDTO Field { get; }

        protected NodeTypeAddFieldCommand()
        {

        }

        public NodeTypeAddFieldCommand(Guid id, FieldDTO field) : this()
        {
            Id = id;
            Field = field;
        }
    }
}
