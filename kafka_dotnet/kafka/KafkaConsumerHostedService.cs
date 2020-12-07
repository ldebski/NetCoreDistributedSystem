using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace kafka
{
    public class KafkaConsumerHostedService : IHostedService
    {
        private ILogger<KafkaConsumerHostedService> _logger;
        private IConsumer<string, string> _consumer;

        public KafkaConsumerHostedService(ILogger<KafkaConsumerHostedService> logger)
        {
            this._logger = logger;
            var config = new ConsumerConfig()
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            this._consumer = new ConsumerBuilder<string, string>(config).Build();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("starting consuming");
            _consumer.Subscribe("out_topic");
            try
            {
                while (true)
                {
                    try
                    {
                        ConsumeResult<string, string> cr = _consumer.Consume(cancellationToken);

                        Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}' with key: {cr.Message.Key}.");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Received shut down info");
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
            }
            finally
            {
                _consumer.Close();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Shutting down consumer");
            _consumer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
