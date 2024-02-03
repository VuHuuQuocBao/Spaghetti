using Confluent.Kafka;
using Newtonsoft.Json;
using Spaghetti.Core.DataTransferObject;
using Spaghetti.Domain.DataTransferObject;
using Spaghetti.Domain.Entity;
using System.Net;
using System.Text;
using static Confluent.Kafka.ConfigPropertyNames;


// need to create topic
// key and value serializer will be set when build producer

var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
    BatchSize = 5,
};

using var producer = new ProducerBuilder<Null, DocumentIndexDto>(config).SetValueSerializer( new JsonSerializer<DocumentIndexDto>()).Build();

// process each individually to simulate real case

for (int i = 0; i < 5; i++)
{
    string jsonString = File.ReadAllText($@"C:\\Users\\PC\\OneDrive\\Desktop\\LastDance\\data\\mesasge_data\\generated{i}.json");
    var myObject = JsonConvert.DeserializeObject<List<Message>>(jsonString)!;

    for (int j = 0; i < myObject.Count; j++)
    {
        var message = myObject[j];
        var documentIndex = new DocumentIndexDto()
        {
            index = "message_discord_ultimate",
            document = message
        };

        var kafkaMessage = new Message<Null, DocumentIndexDto> { Value = documentIndex };


        // push this to kafka

        var result = await producer.ProduceAsync("discord_message", kafkaMessage);
        Console.Write(result);

    }
}


public class JsonSerializer<T> : ISerializer<T>, IDeserializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data);
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(data);
    }
}