using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common.Models;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Command
{
    public class LocalizationHierarchyCommandHandler : IRequestHandler<LocalizationHierarchyCommand, Hierarchy<string>>
    {
        public Task<Hierarchy<string>> Handle(LocalizationHierarchyCommand request, CancellationToken cancellationToken)
        {
            var enUS = new Hierarchy<string>("en-US");
            var esUS = new Hierarchy<string>("es-US");
            var enCA = new Hierarchy<string>("en-CA");
            var frCA = new Hierarchy<string>("fr-CA");
            var esMX = new Hierarchy<string>("es-MX");
            var ptBR = new Hierarchy<string>("pt-BR");

            var en = new Hierarchy<string>("en");
            en.Children.Add(enUS);
            en.Children.Add(enCA);

            var es = new Hierarchy<string>("es");
            es.Children.Add(esUS);
            es.Children.Add(esMX);

            var fr = new Hierarchy<string>("fr");
            fr.Children.Add(frCA);

            var pt = new Hierarchy<string>("pt");
            pt.Children.Add(ptBR);

            var root = new Hierarchy<string>("default");
            root.Children.Add(en);
            root.Children.Add(es);
            root.Children.Add(fr);
            root.Children.Add(pt);

            return Task.FromResult(root);
        }
    }
}
