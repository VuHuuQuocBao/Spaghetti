using Confluent.Kafka;
using System.Net;


var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
    BatchSize = 5,
};


// need to create topic
// key and value serializer will be set when build producer


using (var producer = new ProducerBuilder<Null, string>(config).Build())
{
    for (int i = 0; i < 6; i++)
    {
        var message = new Message<Null, string> { Value = "asdf" + $"{i}" };
        var result = await producer.ProduceAsync("weblog", message);
        Console.Write(result);
    }

    Console.Write("Finished");

}



