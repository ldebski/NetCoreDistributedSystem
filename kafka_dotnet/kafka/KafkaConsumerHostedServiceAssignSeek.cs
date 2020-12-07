using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace kafka
{
    public class KafkaConsumerHostedServiceAssignSeek : IHostedService
    {
        private ILogger<KafkaConsumerHostedServiceAssignSeek> _logger;
        private IConsumer<string, string> _consumer;

        public KafkaConsumerHostedServiceAssignSeek(ILogger<KafkaConsumerHostedServiceAssignSeek> logger)
        {
            this._logger = logger;
            var config = new ConsumerConfig()
            {
                GroupId = "test-assign-seek-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                
            };
            this._consumer = new ConsumerBuilder<string, string>(config).Build();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("starting consuming");
            // _consumer.Subscribe("my_topic");
            TopicPartition partitionToReadFrom = new TopicPartition("my_topic", 1);
            // we can pass here list of partitions as well
            _consumer.Assign(partitionToReadFrom);
            long offsetToReadFrom = 10L;
            _consumer.Seek(new TopicPartitionOffset(partitionToReadFrom, offsetToReadFrom));

            int numberOfMessagesToRead = 5;
            Boolean keepOnReading = true;
            int numberOfMessagesReadSoFar = 0;
            try
            {
                while (keepOnReading)
                {
                    try
                    {
                        ConsumeResult<string, string> cr = _consumer.Consume(cancellationToken);

                        numberOfMessagesReadSoFar++;

                        Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset.Offset}' with key: {cr.Message.Key}.");

                        if (numberOfMessagesReadSoFar >= numberOfMessagesToRead)
                            keepOnReading = false;
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
