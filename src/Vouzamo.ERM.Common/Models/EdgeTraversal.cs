using System;

namespace Vouzamo.ERM.Common
{
    public class EdgeTraversal
    {
        public Guid Id { get; set; }
        public Guid Type { get; set; }
        public Guid Node { get; set; }

        protected EdgeTraversal()
        {

        }

        public EdgeTraversal(Guid id, Guid type, Guid node) : this()
        {
            Id = id;
            Type = type;
            Node = node;
        }
    }
}
