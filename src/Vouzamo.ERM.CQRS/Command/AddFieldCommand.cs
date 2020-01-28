using MediatR;
using System;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.CQRS
{
    public class AddFieldCommand<T> : IRequest<bool> where T : IHasFields, IIdentifiable<Guid>
    {
        public Guid Id { get; }
        public FieldDTO Field { get; }

        protected AddFieldCommand()
        {

        }

        public AddFieldCommand(Guid id, FieldDTO field) : this()
        {
            Id = id;
            Field = field;
        }
    }
}
