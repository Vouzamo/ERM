using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vouzamo.ERM.Common
{
    public interface IGraphProvider
    {
        #region Command
        Task<NodeType> CreateNodeType(string name);
        Task<bool> UpdateNodeType(NodeType nodeType);
        Task<Node> CreateNode(NodeType type, string name);
        Task<bool> UpdateNode(Node node);
        Task<EdgeType> CreateEdgeType(string name);
        Task<Edge> CreateEdge(Node from, EdgeType type, Node to);
        #endregion

        #region Query
        Task<Node> GetNode(Guid id);
        Task<IEnumerable<Node>> QueryNodes(string query, int take = 50, int skip = 0);
        #endregion

        #region Traversal
        Task<IEnumerable<Traversal>> Traverse(Node node, Direction direction);
        Task<IEnumerable<Traversal>> EdgesFrom(Node node);
        Task<IEnumerable<Traversal>> EdgesTo(Node node);
        #endregion
    }
}
