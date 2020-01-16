using System;

namespace Vouzamo.ERM.Common
{
    public class Traversal
    {
        public Guid Id { get; set; }
        public EdgeType Type { get; set; }
        public Direction Direction { get; set; }
        public Node Node { get; set; }

        protected Traversal()
        {

        }

        public Traversal(Guid id, EdgeType type, Direction direction, Node node) : this()
        {
            Id = id;
            Type = type;
            Direction = direction;
            Node = node;
        }
    }
}
