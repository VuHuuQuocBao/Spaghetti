using Newtonsoft.Json;
using Spaghetti.Core.DataTransferObject;
using Spaghetti.Domain.Entity;
using StackExchange.Redis;
using System.Text;




while (true)
{

    ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
    var db = redis.GetDatabase();
    var endpoints = redis.GetEndPoints();
    var server = redis.GetServer(endpoints.First());
    var keys = server.Keys().Take(100).ToList();

    if(keys is { Count: >= 100 })
    {
        // needs to replace to batch get but still can't find
        var keyValuePairs = new Dictionary<string, string>();

        foreach (var key in keys)
        {
            var value = db.StringGet(key);
            keyValuePairs.Add(key, value);
        }

        // bulk index, hard code cause i don't want to think
        var documentsIndex = new DocumentsIndexDto();
        documentsIndex.index = "message_discord_ultimate";
        documentsIndex.documents = new List<Message>();

        HttpClient client = new HttpClient();
        foreach (var kv in keyValuePairs)
        {
            // key is not important here
            var gTemp = JsonConvert.DeserializeObject<Message>(kv.Value);
            documentsIndex.documents.Add(gTemp);
        }

        // why can't convert page to object

        var content = new StringContent(JsonConvert.SerializeObject(documentsIndex), Encoding.UTF8, "application/json");

        Task indexTask = client.PostAsync("https://localhost:7166/IndexDocuments", content);

        Task.WaitAll(indexTask);

    }
    Thread.Sleep(5000);
}




var a = 1;