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
            Console.WriteLine("Consuming Queue Now");

            // creating connection to database
            DataBaseService dataBaseService = new DataBaseService();

            ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: "get",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            Console.WriteLine("Consuming Queue Now");

            consumer.Received += (model, ea) =>
            {
                string response = "";

                var body = ea.Body;
                // Console.WriteLine("got message: " + Encoding.UTF8.GetString(body));

                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();

                try
                {
                    var message = Encoding.UTF8.GetString(body).Split(".");
                    response = dataBaseService.Query(message[1]);
                    response = message[0] + "." + response;
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                    var message = Encoding.UTF8.GetString(body).Split(".");
                    response = message[0] + "." + "error";
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish(exchange: "",
                                         routingKey: props.ReplyTo,
                                         basicProperties: replyProps,
                                         body: responseBytes);
                    Console.WriteLine("Done");
                }
            };

            channel.BasicConsume(queue: "get",
                                    autoAck: true,
                                    consumer: consumer);
        }
    }
}