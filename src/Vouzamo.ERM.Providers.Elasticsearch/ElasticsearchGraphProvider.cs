using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Node = Vouzamo.ERM.Common.Node;

namespace Vouzamo.ERM.Providers.Elasticsearch
{
    public sealed class ElasticsearchGraphProvider : IGraphProvider
    {
        private IElasticClient Client { get; }

        public ElasticsearchGraphProvider()
        {
            var connectionPool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(connectionPool)
                .DisableDirectStreaming()
                .PrettyJson()
                .OnRequestCompleted(response =>
                {
                    // log out the request
                    if (response.RequestBodyInBytes != null)
                    {
                        Console.WriteLine(
                            $"{response.HttpMethod} {response.Uri} \n" +
                            $"{Encoding.UTF8.GetString(response.RequestBodyInBytes)}");
                    }
                    else
                    {
                        Console.WriteLine($"{response.HttpMethod} {response.Uri}");
                    }

                    // log out the response
                    if (response.ResponseBodyInBytes != null)
                    {
                        Console.WriteLine($"Status: {response.HttpStatusCode}\n" +
                                 $"{Encoding.UTF8.GetString(response.ResponseBodyInBytes)}\n" +
                                 $"{new string('-', 30)}\n");
                    }
                    else
                    {
                        Console.WriteLine($"Status: {response.HttpStatusCode}\n" +
                                 $"{new string('-', 30)}\n");
                    }
                })
                .DefaultIndex("nodes");

            Client = new ElasticClient(connectionSettings);
        }

        public async Task<NodeType> CreateNodeType(string name)
        {
            var document = new NodeType(Guid.NewGuid(), name);

            var response = await Client.IndexAsync(document, descriptor => descriptor
                .Index("node-types")
                .Id(document.Id)
            );

            if(!response.IsValid)
            {
                // shit!
            }

            return document;
        }

        public async Task<bool> UpdateNodeType(NodeType nodeType)
        {
            var response = await Client.IndexAsync(nodeType, descriptor => descriptor
                .Index("node-types")
                .Id(nodeType.Id)
            );

            return response.IsValid;
        }

        public async Task<Node> CreateNode(NodeType type, string name)
        {
            var document = new Node(Guid.NewGuid(), type.Id, name);

            var response = await Client.IndexAsync(document, descriptor => descriptor
                .Index("nodes")
                .Id(document.Id)
            );

            if (!response.IsValid)
            {
                // shit!
            }

            return document;
        }

        public async Task<bool> UpdateNode(Node node)
        {
            var response = await Client.IndexAsync(node, descriptor => descriptor
                .Index("nodes")
                .Id(node.Id)
            );

            return response.IsValid;
        }

        public async Task<EdgeType> CreateEdgeType(string name)
        {
            var document = new EdgeType(Guid.NewGuid(), name);

            var response = await Client.IndexAsync(document, descriptor => descriptor
                .Index("edge-types")
                .Id(document.Id)
            );

            if (!response.IsValid)
            {
                // shit!
            }

            return document;
        }

        public async Task<Edge> CreateEdge(Node from, EdgeType type, Node to)
        {
            var document = new Edge(Guid.NewGuid(), type.Id, from.Id, to.Id);

            var response = await Client.IndexAsync(document, descriptor => descriptor
                .Index("edges")
                .Id(document.Id)
            );

            if (!response.IsValid)
            {
                // shit!
            }

            return document;
        }

        public async Task<Node> GetNode(Guid id)
        {
            var response = await Client.GetAsync<Node>(id, descriptor => descriptor
                .Index("nodes")
            );

            if (!response.IsValid)
            {
                // shit!
            }

            return response.Source;
        }

        public async Task<IEnumerable<Node>> QueryNodes(string query, int take = 50, int skip = 0)
        {
            var response = await Client.SearchAsync<Node>(descriptor => descriptor
                .Index("nodes")
                .Query(q => q.MatchAll())
                .Skip(skip)
                .Size(take)
            );

            if (!response.IsValid)
            {
                // shit!
            }

            return response.Documents;
        }

        public async Task<IEnumerable<Traversal>> Traverse(Node node, Direction direction)
        {
            var response = await Client.SearchAsync<Edge>(descriptor => descriptor
                .Index("edges")
                .Query(q => q
                    .Term(t => t
                        .Field(direction.Equals(Direction.Outbound) ? "from.keyword" : "to.keyword")
                        .Value(node.Id)
                    )
                )
            );

            if (!response.IsValid)
            {
                // shit!
            }

            var results = response.Documents.Select(edge => {
                
                var typeResponse = Client.Get<EdgeType>(edge.Type, descriptor => descriptor
                    .Index("edge-types")
                );

                var edgeType = typeResponse.Source;

                var destinationNode = GetNode(direction.Equals(Direction.Outbound) ? edge.To : edge.From).Result;

                return new Traversal(edge.Id, edgeType, direction, destinationNode);
            });

            return results.ToList();
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
