using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS.Command
{
    public class RenameCommand<T> : IRequest<bool> where T : IHasName
    {
        public Guid Id { get; }
        public string Name { get; }

        public RenameCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
