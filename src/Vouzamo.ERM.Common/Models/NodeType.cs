using System;
using System.Collections.Generic;
using System.Linq;

namespace Vouzamo.ERM.Common
{
    public class NodeType : IEquatable<NodeType>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }

        protected NodeType()
        {
            Fields = new List<Field>();
        }

        public NodeType(Guid id, string name) : this()
        {
            Id = id;
            Name = name;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return Equals(other as NodeType);
        }

        public bool Equals(NodeType other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   Name.Equals(other.Name) &&
                   Fields.SequenceEqual(other.Fields);
        }
    }
}
