







using Spaghetti.Core;
using Newtonsoft.Json;
using Spaghetti.Core.DataTransferObject;
using System.Text;
using System;

HttpClient client = new HttpClient();

string jsonString = File.ReadAllText(@"C:\\Users\\PC\\OneDrive\\Desktop\\LastDance\\data\\mesasge_data\\generated4.json");

var myObject = JsonConvert.DeserializeObject<List<Message>>(jsonString)!;


/*myObject.ForEach(_ =>
{
    var documentIndexInstance = new DocumentIndexDto()
    {
        index = "message_discord",
        document = new Message
        {
            author_id = _.author_id,
            message_id = _.message_id,
            channel_id = _.channel_id,
            content = _.content,
            server_id = _.server_id,
        }
    };

    // post
    var content = new StringContent(JsonConvert.SerializeObject(documentIndexInstance), Encoding.UTF8, "application/json");

    Task indexTask = client.PostAsync("https://localhost:7166/IndexDocument", content);

    Task.WaitAll(indexTask);

    Thread.Sleep(500);

});
*/

int pageSize = 10000; // Number of items per page

for (int i = 0; i < myObject.Count; i += pageSize)
{
    List<Message> page = myObject.Skip(i).Take(pageSize).ToList();
    var documentsIndex = new DocumentsIndexDto()
    {
        index = "message_discord1",
        documents = page
    };
    // why can't convert page to object

    var content = new StringContent(JsonConvert.SerializeObject(documentsIndex), Encoding.UTF8, "application/json");

    Task indexTask = client.PostAsync("https://localhost:7166/IndexDocuments", content);

    Task.WaitAll(indexTask);

    Thread.Sleep(1000);
}

