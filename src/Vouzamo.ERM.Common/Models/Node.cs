using System;
using System.Collections.Generic;

namespace Vouzamo.ERM.Common
{
    public class Node
    {
        public Guid Id { get; set; }
        public Guid Type { get; set; }
        public string Name { get; set; }
        public Dictionary<string, LocalizedValue> Properties { get; set; }

        protected Node()
        {
            Properties = new Dictionary<string, LocalizedValue>();
        }

        public Node(Guid id, Guid type, string name) : this()
        {
            Id = id;
            Type = type;
            Name = name;
        }
    }
}
