using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace kafka
{
    public class KafkaProducerHostedService
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducerHostedService()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "kafka:29092",

                // ensure safety of producer
                Acks = Acks.All,
                EnableIdempotence = true, // idempotence producer, no data will be duplicated
                MessageSendMaxRetries = int.MaxValue,
                MaxInFlight = 5,

                //message compression, important for big messages or batches of messages
                CompressionType = CompressionType.Snappy,

                // messages batching
                LingerMs = 20, //how much time producer will wait before sending a batch
                BatchSize = 1024 * 1024 // (1mb) how much bytes can be send at once
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async void StartAsync(int val, string guid, string from, string to)
        {
            var transaction = new Transaction(guid, to, val, val);
            var valJson = JsonConvert.SerializeObject(transaction);

            var key = from;
            var topic = "firstTopic";
            await Console.Out.WriteLineAsync(valJson);
            await Console.Out.WriteLineAsync("Key: " + key);
            var deliveryResult = await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Value = valJson,
                Key = key
            });

            // informacja zwrotna
            await Console.Out.WriteLineAsync("received data\n" +
                                             "Topic: " + deliveryResult.Topic +
                                             "\nPartition: " + deliveryResult.Partition +
                                             "\noffset: " + deliveryResult.Offset +
                                             "\ntimestamp: " + deliveryResult.Timestamp);


            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}