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
                MaxInFlight = 5
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async void StartAsync(string guid, string accountId)
        {
            var transaction = new Transaction(guid);
            var valJson = JsonConvert.SerializeObject(transaction);

            var key = accountId;
            var topic = "firstTopic";
            var deliveryResult = await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Value = valJson,
                Key = key
            });

            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}