﻿using MediatR;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.CQRS;

namespace Vouzamo.ERM.Providers.Elasticsearch.Handlers.Query
{
    public class NodesByNodeTypesQueryHandler : IRequestHandler<NodesByNodeTypesQuery, ILookup<Guid, Node>>
    {
        protected IElasticClient Client { get; }

        public NodesByNodeTypesQueryHandler(IElasticClient client)
        {
            Client = client;
        }

        public async Task<ILookup<Guid, Node>> Handle(NodesByNodeTypesQuery request, CancellationToken cancellationToken)
        {
            var response = await Client.SearchAsync<Node>(descriptor => descriptor
                .Query(q => q
                    .Terms(t => t
                        .Field(f => f.Type.Suffix("keyword"))
                        .Terms(request.NodeTypes)
                    )
                )
                .Skip(request.Skip)
                .Size(request.Take)
            );

            if (!response.IsValid)
            {
                throw response.OriginalException;
            }

            return response.Documents.ToLookup(node => node.Type);
        }
    }
}
