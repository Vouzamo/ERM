using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class UpdateCommand<T> : IRequest<T> where T : class, IIdentifiable<Guid>
    {
        public T Updated { get; }

        public UpdateCommand(T updated)
        {
            Updated = updated;
        }
    }
}
