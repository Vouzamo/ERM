using MediatR;
using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class UpdateFieldsCommand<T> : IRequest<T> where T : IHasFields, IIdentifiable<Guid>
    {
        public Guid Id { get; }
        public List<Field> Fields { get; }

        protected UpdateFieldsCommand()
        {

        }

        public UpdateFieldsCommand(Guid id, List<Field> fields) : this()
        {
            Id = id;
            Fields = fields;
        }
    }
}
