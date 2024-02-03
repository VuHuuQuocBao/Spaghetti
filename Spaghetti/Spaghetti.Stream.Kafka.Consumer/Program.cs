using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using Newtonsoft.Json;
using Spaghetti.Domain.DataTransferObject;
using Spaghetti.Domain.Entity;
using StackExchange.Redis;


var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "discord_message_consumer",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

// connect redis

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
IDatabase db = redis.GetDatabase();

using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
{
    consumer.Subscribe("discord_message");

    while (true)
    {
        var consumeResult = consumer.Consume();
        var message = JsonConvert.DeserializeObject<DocumentIndexDto>(consumeResult.Value);

        var document = JsonConvert.DeserializeObject<Message>(JsonConvert.SerializeObject(message.document));

        // push to redis directly

        db.StringSet(document.message_id.ToString(), JsonConvert.SerializeObject(message.document));
        string value = db.StringGet(document.message_id.ToString());
        Console.WriteLine(value);  // Outputs: myvalue

        Console.WriteLine(consumeResult);
    }
}