using System;
using System.Collections.Generic;

namespace Vouzamo.ERM.Common
{
    public class Edge
    {
        public Guid Id { get; set; }
        public Guid Type { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
        public Dictionary<string, LocalizedValue> Properties { get; set; }

        protected Edge()
        {
            Properties = new Dictionary<string, LocalizedValue>();
        }

        public Edge(Guid id, Guid type, Guid from, Guid to) : this()
        {
            Id = id;
            Type = type;
            From = from;
            To = to;
        }
}
}
