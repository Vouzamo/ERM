using GraphQL.Authorization;
using System.Collections.Generic;
using System.Security.Claims;

namespace Vouzamo.ERM.Api.Graph.Types.Fields
{
    public class GraphQLUserContext : Dictionary<string, object>, IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}
