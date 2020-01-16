using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.Providers.InMemory
{
    public sealed class InMemoryGraphProvider : IGraphProvider
    {
        private List<NodeType> NodeTypes { get; } = new List<NodeType>();
        private List<Node> Nodes { get; } = new List<Node>();
        private List<EdgeType> EdgeTypes { get; } = new List<EdgeType>();
        private List<Edge> Edges { get; } = new List<Edge>();
         
        public InMemoryGraphProvider()
        {

        }

        public Task<NodeType> CreateNodeType(string name)
        {
            var nodeType = new NodeType(Guid.NewGuid(), name);

            NodeTypes.Add(nodeType);

            return Task.FromResult(nodeType);
        }

        public Task<bool> UpdateNodeType(NodeType nodeType)
        {
            // object is already in-memory and updated by reference
            return Task.FromResult(true);
        }

        public Task<Node> CreateNode(NodeType type, string name)
        {
            var node = new Node(Guid.NewGuid(), type.Id, name);

            Nodes.Add(node);

            return Task.FromResult(node);
        }

        public Task<bool> UpdateNode(Node node)
        {
            // object is already in-memory and updated by reference
            return Task.FromResult(true);
        }

        public Task<EdgeType> CreateEdgeType(string name)
        {
            var edgeType = new EdgeType(Guid.NewGuid(), name);

            EdgeTypes.Add(edgeType);

            return Task.FromResult(edgeType);
        }

        public Task<Edge> CreateEdge(Node from, EdgeType type, Node to)
        {
            var edge = new Edge(Guid.NewGuid(), type.Id, from.Id, to.Id);

            Edges.Add(edge);

            return Task.FromResult(edge);
        }

        public Task<Node> GetNode(Guid id)
        {
            return Task.FromResult(Nodes.SingleOrDefault(node => node.Id.Equals(id)));
        }

        public Task<IEnumerable<Node>> QueryNodes(string query, int take = 50, int skip = 0)
        {
            return Task.FromResult(Nodes.Skip(skip).Take(take));
        }

        public Task<IEnumerable<Traversal>> Traverse(Node node, Direction direction)
        {
            IEnumerable<Edge> edges;

            if(direction.Equals(Direction.Outbound))
            {
                edges = Edges.Where(edge => edge.From.Equals(node.Id));
            }
            else
            {
                edges = Edges.Where(edge => edge.To.Equals(node.Id));
            }

            return Task.FromResult(edges.Select(edge =>
            {
                var edgeType = EdgeTypes.SingleOrDefault(type => type.Id.Equals(edge.Type));
                var destinationNode = GetNode(direction.Equals(Direction.Outbound) ? edge.To : edge.From).Result;

                return new Traversal(edge.Id, edgeType, direction, destinationNode);
            }));
        }

        public async Task<IEnumerable<Traversal>> EdgesFrom(Node node)
        {
            return await Traverse(node, Direction.Outbound);
        }

        public async Task<IEnumerable<Traversal>> EdgesTo(Node node)
        {
            return await Traverse(node, Direction.Inbound);
        }
    }
}
