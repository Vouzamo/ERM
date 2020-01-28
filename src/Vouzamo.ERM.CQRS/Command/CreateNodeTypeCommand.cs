using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class CreateTypeCommand : IRequest<Common.Type>
    {
        public Guid Id { get; }
        public string Name { get; }
        public TypeScope Scope { get; }

        public CreateTypeCommand(string name, TypeScope scope, Guid? id = null)
        {
            Name = name;
            Scope = scope;

            if(!id.HasValue)
            {
                id = Guid.NewGuid();
            }

            Id = id.Value;
        }
    }
}
