using System;
using System.Text;
using RabbitMQ.Client;

namespace sender.Services
{
    public interface IMessageService
    {
        bool Enqueue(string message, string queueName);
    }

    public class MessageService : IMessageService
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;
        uint counter;
        public MessageService()
        {
            Console.WriteLine("about to connect to rabbit");

            counter = 0;
            _factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            _factory.UserName = "guest";
            _factory.Password = "guest";
            _conn = _factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.QueueDeclare(queue: "przelew",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            _channel.QueueDeclare(queue: "get",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            Console.WriteLine("connected to rabbit");
        }
        public bool Enqueue(string messageString, string queueName)
        {
            var body = Encoding.UTF8.GetBytes(messageString);
            _channel.BasicPublish(exchange: "",
                            routingKey: queueName,
                            basicProperties: _channel.CreateBasicProperties(),
                            body: body);
            // Console.WriteLine(" [x] Published {0} to {1} queue", messageString, queueName);
            Console.WriteLine($" [x] Published {counter}");
            counter++;
            return true;
        }
    }
}