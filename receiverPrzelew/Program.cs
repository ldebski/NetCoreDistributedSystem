using System;
using System.Text;
using System.Threading.Tasks;
using kafka;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace receiver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task.Delay(20000).Wait(); // waiting for rabbit & sql
            // DataBaseService dataBaseService = new DataBaseService();
            var kafkaProducerHostedService = new KafkaProducerHostedService();

            var factory = new ConnectionFactory {HostName = "rabbitmq", Port = 5672};
            factory.UserName = "guest";
            factory.Password = "guest";
            var conn = factory.CreateConnection();
            var channel = conn.CreateModel();
            channel.QueueDeclare("przelew",
                false,
                false,
                false,
                null);

            var consumer = new EventingBasicConsumer(channel);

            Console.WriteLine("Consuming Queue Now");

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    var msgSplit = message.Split(".");
                    Console.Out.WriteLine("I'm putting message from: " + msgSplit[1] + " to kafka");
                    kafkaProducerHostedService.StartAsync(-int.Parse(msgSplit[3]), msgSplit[0], msgSplit[1],
                        msgSplit[2]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                    var message = Encoding.UTF8.GetString(body).Split(".");
                }
            };

            channel.BasicConsume("przelew",
                true,
                consumer);
        }
    }
}