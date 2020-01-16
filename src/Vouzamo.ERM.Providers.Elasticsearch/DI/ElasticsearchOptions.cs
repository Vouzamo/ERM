using System;
using System.Collections.Generic;
using System.Text;

namespace Vouzamo.ERM.Providers.Elasticsearch.DI
{
    public class ElasticsearchOptions
    {
        public Uri Uri { get; }

        public ElasticsearchOptions(Uri uri)
        {
            Uri = uri;
        }

        public static ElasticsearchOptions Default => new ElasticsearchOptions(new Uri("http://localhost:9200"));
    }
}
