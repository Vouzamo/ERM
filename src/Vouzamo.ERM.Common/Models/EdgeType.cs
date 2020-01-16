using System;
using System.Collections.Generic;

namespace Vouzamo.ERM.Common
{
    public class EdgeType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }

        protected EdgeType()
        {
            Fields = new List<Field>();
        }

        public EdgeType(Guid id, string name) : this()
        {
            Id = id;
            Name = name;
        }
    }
}
