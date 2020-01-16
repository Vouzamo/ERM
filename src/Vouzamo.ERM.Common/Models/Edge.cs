using System;
using System.Collections.Generic;

namespace Vouzamo.ERM.Common
{
    public class Edge
    {
        public Guid Id { get; protected set; }
        public Guid Type { get; protected set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
        public Dictionary<string, LocalizedValue> Properties { get; set; }

        public Edge(Guid id, Guid type, Guid from, Guid to)
        {
            Id = id;
            Type = type;
            From = from;
            To = to;
            Properties = new Dictionary<string, LocalizedValue>();
        }
}
}
