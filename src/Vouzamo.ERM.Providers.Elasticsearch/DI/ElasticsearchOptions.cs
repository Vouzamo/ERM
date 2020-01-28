using System;

namespace Vouzamo.ERM.Providers.Elasticsearch.DI
{
    public class ElasticsearchOptions
    {
        public Uri Uri { get; set; }

        public string TypesIndex { get; set; }
        public string NodesIndex { get; set; }
        public string EdgesIndex { get; set; }

        public ElasticsearchOptions(Uri uri)
        {
            Uri = uri;

            TypesIndex = "erm.types";
            NodesIndex = "erm.nodes";
            EdgesIndex = "erm.edges";
        }

        public static ElasticsearchOptions Default => new ElasticsearchOptions(new Uri("http://localhost:9200"));
    }
}
