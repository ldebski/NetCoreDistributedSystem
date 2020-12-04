using System;
using Confluent.Kafka;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/*
 * docker-compose exec broker kafka-topics \
  --create \
  --bootstrap-server localhost:9092 \
  --replication-factor 1 \
  --partitions 1 \
  --topic users
 */


namespace kafka
{
    class Program
    {
        static void Main(string[] args)
        {
            createHostBuilder(args).Build().Run();
        }

        private static IHostBuilder createHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, collection) =>
                {
                    collection.AddHostedService<KafkaConsumerHostedService>();
                    collection.AddHostedService<KafkaProducerHostedService>();
                });
    }
    
    public class KafkaConsumerHostedService : IHostedService
    {
        private ILogger<KafkaConsumerHostedService> _logger;
        private IConsumer<Ignore, string> _consumer;

        public KafkaConsumerHostedService(ILogger<KafkaConsumerHostedService> logger)
        {
            this._logger = logger;
            var config = new ConsumerConfig()
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092"
            };
            this._consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("demo");
            try
            {
                while (true)
                {
                    try
                    {
                        var cr = _consumer.Consume(cancellationToken);
                        Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                _consumer.Close();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer?.Dispose();
            return Task.CompletedTask;
        }
    }

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
                Acks = Acks.All,
                
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (var i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                var value = $"Hello {i}";
                var key = $"id_{i}";
                var topic = "demo";
                this._logger.LogInformation(value);
                this._logger.LogInformation("Key: "+ key);
                
                await _producer.ProduceAsync(topic, new Message<string, string>()
                {
                    Value = value,
                    Key = key
                    
                }, cancellationToken); // callback: https://docs.microsoft.com/en-us/dotnet/api/system.asynccallback?view=net-5.0
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