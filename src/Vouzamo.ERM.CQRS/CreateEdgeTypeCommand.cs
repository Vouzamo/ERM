using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class CreateEdgeTypeCommand : IRequest<EdgeType>
    {
        public Guid Id { get; }
        public string Name { get; }

        public CreateEdgeTypeCommand(string name, Guid? id = null)
        {
            Name = name;

            if (!id.HasValue)
            {
                id = Guid.NewGuid();
            }

            Id = id.Value;
        }
    }
}
