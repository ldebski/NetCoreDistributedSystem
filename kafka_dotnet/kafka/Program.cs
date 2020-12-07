using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


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
                    // collection.AddHostedService<KafkaConsumerHostedService>();
                    collection.AddHostedService<KafkaProducerHostedService>();
                    // collection.AddHostedService<KafkaConsumerHostedServiceAssignSeek>();
                });
    }
}