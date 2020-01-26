using System;

namespace Vouzamo.ERM.Providers.Elasticsearch.DI
{
    public class ElasticsearchOptions
    {
        public Uri Uri { get; set; }

        public string NodesIndex { get; set; }
        public string NodeTypesIndex { get; set; }
        public string EdgesIndex { get; set; }
        public string EdgeTypesIndex { get; set; }

        public ElasticsearchOptions(Uri uri)
        {
            Uri = uri;

            NodesIndex = "erm.nodes";
            NodeTypesIndex = "erm.nodetypes";
            EdgesIndex = "erm.edges";
            EdgeTypesIndex = "erm.edgetypes";
        }

        public static ElasticsearchOptions Default => new ElasticsearchOptions(new Uri("http://localhost:9200"));
    }
}
