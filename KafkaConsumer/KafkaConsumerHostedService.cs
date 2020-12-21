using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json.Linq;

namespace KafkaConsumer
{
    public class KafkaConsumerHostedService
    {
        private readonly IConsumer<string, string> _consumer;

        public KafkaConsumerHostedService()
        {
            var config = new ConsumerConfig
            {
                GroupId = "consumer-group",
                BootstrapServers = "kafka:29092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        public Task StartAsync()
        {
            var messageService = new MessageService();
            _consumer.Subscribe("secondTopic");
            try
            {
                while (true)
                    try
                    {
                        // {"guid":"404a2be4-e381-4214-87ff-16eafd3ca2ab","toID":"2","value":-123,"ret":"false","sum":60389,"done":"true"}
                        var cr = _consumer.Consume();

                        dynamic transaction = JObject.Parse(cr.Message.Value);

                        Console.WriteLine("Consumed message from kafka for user: " + cr.Message.Key);
                        // Console.WriteLine("Sending back massage: " + transaction.guid + "."
                        //                   + "przelew_" + transaction.done + "_na_kwote:" + transaction.value +
                        //                   "_suma_na_koncie:" + transaction.sum);

                        messageService.Enqueue(
                            transaction.guid.ToString() + "." + transaction.done.ToString() + "_" +
                            transaction.sum.ToString());
                        // + "przelew_" + transaction.done + "_na_kwote:" + transaction.value +
                        // "_suma_na_koncie:" + transaction.sum);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
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