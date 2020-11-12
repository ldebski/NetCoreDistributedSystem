using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using receiverGet.Services;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Delay(20000).Wait(); // waiting for rabbit & sql
            // creating connection to database
            DataBaseService dataBaseService = new DataBaseService();
            Console.WriteLine("Consuming Queue Now");

            ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: "przelew",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                // Console.WriteLine(" [x] Received from Rabbit: {0}", message);
                dataBaseService.Query(message);
                Console.WriteLine(" [x] Done processing message from Rabbit : {0}", message);
            };
            channel.BasicConsume(queue: "przelew",
                                    autoAck: true,
                                    consumer: consumer);

        }
    }
}
