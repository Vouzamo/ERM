﻿using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class NodesBySearchQuery : IRequest<BatchedResults<Node>>
    {
        public string Query { get; }
        public int Size { get; }
        public int Page { get; }

        public NodesBySearchQuery(string query, int size, int page = 1)
        {
            Query = query;
            Size = Math.Max(1, size);
            Page = Math.Max(1, page);
        }
    }
}
