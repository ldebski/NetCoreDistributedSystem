using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kafka
{
    public class KafkaProducerHostedService : IHostedService
    {
        private readonly ILogger<KafkaProducerHostedService> _logger;
        private IProducer<string, string> _producer;

        public KafkaProducerHostedService(ILogger<KafkaProducerHostedService> logger)
        {
            this._logger = logger;
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",

                // ensure safety of producer
                Acks = Acks.All,
                EnableIdempotence = true, // idempotence producer, no data will be duplicated
                MessageSendMaxRetries = int.MaxValue,
                MaxInFlight = 5,

                //message compression, important for big messages or batches of messages
                CompressionType = CompressionType.Snappy,

                // messages batching
                LingerMs = 20, //how much time producer will wait before sending a batch
                BatchSize = 1024*1024, // (1mb) how much bytes can be send at once

                
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            int[] vs = { -200 };
            for (var i = 0; i < 1; i++)
            {
                Thread.Sleep(100);
                Transaction transaction = new Transaction("1", "2", vs[i], 0, "false");
            
                string value = JsonConvert.SerializeObject(transaction);
                var key = $"id37";
                var topic = "firstTopic";
                this._logger.LogInformation(value);
                this._logger.LogInformation("Key: " + key);
                DeliveryResult<string, string> deliveryResult = await _producer.ProduceAsync(topic, new Message<string, string>()
                {
                    Value = value,
                    Key = key

                }, cancellationToken);

                _logger.LogInformation("received data\n" +
                    "Topic: " + deliveryResult.Topic +
                    "\nPartition: " + deliveryResult.Partition +
                    "\noffset: " + deliveryResult.Offset +
                    "\ntimestamp: " + deliveryResult.Timestamp.ToString());
            }

            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }

    }
}
