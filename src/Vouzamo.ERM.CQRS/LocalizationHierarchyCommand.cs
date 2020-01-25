using MediatR;
using Vouzamo.ERM.Common.Models;

namespace Vouzamo.ERM.CQRS
{
    public class LocalizationHierarchyCommand : IRequest<Hierarchy<string>>
    {

    }
}
