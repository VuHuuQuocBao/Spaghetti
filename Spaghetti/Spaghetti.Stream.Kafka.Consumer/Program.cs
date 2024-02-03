using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;


var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "Draven",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
{
    consumer.Subscribe("weblog");

    while (true)
    {
        var consumeResult = consumer.Consume();

        Console.WriteLine(consumeResult);

        var a = 1;
        // handle consumed message.
    }

    consumer.Close();
}