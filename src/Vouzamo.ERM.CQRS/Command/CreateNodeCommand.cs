using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class CreateNodeCommand : IRequest<Node>
    {
        public Guid Id { get; }
        public Guid Type { get; }
        public string Name { get; }

        public CreateNodeCommand(Guid type, string name, Guid? id = null)
        {
            Type = type;
            Name = name;

            if (!id.HasValue)
            {
                id = Guid.NewGuid();
            }

            Id = id.Value;
        }
    }
}
