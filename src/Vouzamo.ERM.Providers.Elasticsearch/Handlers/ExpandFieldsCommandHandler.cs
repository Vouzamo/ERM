using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Models;
using Vouzamo.ERM.CQRS;
using Vouzamo.ERM.CQRS.Extensions;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class ExpandFieldsCommandHandler : IRequestHandler<ExpandFieldsCommand, IEnumerable<Hierarchy<Field>>>
    {
        private IMediator Mediator { get; }

        public ExpandFieldsCommandHandler(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task<IEnumerable<Hierarchy<Field>>> Handle(ExpandFieldsCommand request, CancellationToken cancellationToken)
        {
            return await request.Source.Fields.ExpandFields(Mediator);
        }
    }
}
