using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Elasticsearch.Net;
using Spaghetti.Core;
using Spaghetti.Core.DataTransferObject;
using Spaghetti.ElasticSearch.Abstractions;

namespace Spaghetti.ElasticSearch.ElasicSearch
{
    public class ElasticSerachService : IElasticSearchService
    {

        public async void InsertIndex(DocumentIndexDto instance)
        {
            var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
              
                .CertificateFingerprint("C9:29:AE:F3:DF:F6:01:0D:F0:B8:7F:EC:AD:AF:38:6F:30:4A:FD:D5:D7:81:59:96:04:E2:F3:7F:78:E2:6E:A9")
                .Authentication(new BasicAuthentication("elastic", "aMoU3FUoSyj*-k76f0MJ"));
            var client = new ElasticsearchClient(settings);
            //var res = await client.Indices.CreateAsync("quocbao");
            var resDoc = await client.IndexAsync<object>(instance.document, instance.index);
            

            //var resDocs = await client.IndexManyAsync<IEnumerable<object>>()

        }

        public async void InsertManyIndex(DocumentsIndexDto instance)
        {
            var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))

                .CertificateFingerprint("C9:29:AE:F3:DF:F6:01:0D:F0:B8:7F:EC:AD:AF:38:6F:30:4A:FD:D5:D7:81:59:96:04:E2:F3:7F:78:E2:6E:A9")
                .Authentication(new BasicAuthentication("elastic", "aMoU3FUoSyj*-k76f0MJ"));
            var client = new ElasticsearchClient(settings);

            var resDocs = await client.IndexManyAsync<Message>(instance.documents, instance.index);

        }

        public async Task<object> ESSearch(string keyword)
        {
            var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))

                .CertificateFingerprint("C9:29:AE:F3:DF:F6:01:0D:F0:B8:7F:EC:AD:AF:38:6F:30:4A:FD:D5:D7:81:59:96:04:E2:F3:7F:78:E2:6E:A9")
                .Authentication(new BasicAuthentication("elastic", "aMoU3FUoSyj*-k76f0MJ"));
            var client = new ElasticsearchClient(settings);

            /*var resDocs = await client.SearchAsync<Message>(s => s.Index("message_discord1")
                                                        .From(0).Size(100).Query(q => q.Term(t => t.content, keyword)));
*/

            var resDocs = await client.SearchAsync<Message>(s => s
    .Index("message_discord1")
    .From(0)
    .Size(100)
    .Query(q => q
        .Match(m => m
            .Field(f => f.content)
            .Query(keyword)
        )
    )
);

            var result = new List<Message>();

            foreach ( var doc in resDocs.Hits) 
            {
                result.Add(doc.Source);
            }

            return result;
        }
    }
}

