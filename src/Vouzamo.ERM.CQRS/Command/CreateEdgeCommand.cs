using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class CreateEdgeCommand : IRequest<Edge>
    {
        public Guid Id { get; }
        public Guid Type { get; }
        public Guid From { get; }
        public Guid To { get; }

        public CreateEdgeCommand(Guid type, Guid from, Guid to, Guid? id = null)
        {
            Type = type;
            From = from;
            To = to;

            if (!id.HasValue)
            {
                id = Guid.NewGuid();
            }

            Id = id.Value;
        }
    }
}
