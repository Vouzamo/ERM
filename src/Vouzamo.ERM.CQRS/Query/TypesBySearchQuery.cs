using MediatR;
using System;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.CQRS
{
    public class TypesBySearchQuery : IRequest<BatchedResults<Common.Type>>
    {
        public string Query { get; }
        public TypeScope? Scope { get; }
        public int Size { get; }
        public int Page { get; }

        public TypesBySearchQuery(string query, TypeScope? scope, int size, int page = 1)
        {
            Query = query;
            Scope = scope;
            Size = Math.Max(1, size);
            Page = Math.Max(1, page);
        }
    }
}
