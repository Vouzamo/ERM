using MediatR;
using Nest;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class CreateTypeCommandHandler : IRequestHandler<CreateTypeCommand, Common.Type>
    {
        protected IElasticClient Client { get; }

        public CreateTypeCommandHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<Common.Type> Handle(CreateTypeCommand request, CancellationToken cancellationToken)
        {
            var document = new Common.Type(request.Id, request.Name, request.Scope);

            var response = await Client.CreateDocumentAsync(document, cancellationToken);

            if (!response.IsValid)
            {
                // todo: Wrap this in an application exception
                throw response.OriginalException;
            }

            return document;
        }
    }
}
