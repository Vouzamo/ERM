using MediatR;
using System.Collections.Generic;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Models;

namespace Vouzamo.ERM.CQRS
{
    public class ExpandFieldsCommand : IRequest<IEnumerable<Hierarchy<Field>>>
    {
        public IHasFields Source { get; }

        public ExpandFieldsCommand(IHasFields source)
        {
            Source = source;
        }
    }
}
