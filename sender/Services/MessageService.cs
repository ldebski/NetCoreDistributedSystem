using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace sender.Services
{
    public interface IMessageService
    {
        bool Enqueue(string message, string queueName);
    }

    public class MessageService : IMessageService
    {
        readonly IModel _channel;
        private readonly string replyQueueName;
        private readonly IBasicProperties props;
        public MessageService(IRabbitService rabbitService)
        {
            _channel = rabbitService.GetChannel();

            replyQueueName = rabbitService.GetReplyQueueName();
            _channel.QueueDeclare(replyQueueName,
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

            props = _channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;

            Console.WriteLine("Created messages handler");
        }
        public bool Enqueue(string messageString, string queueName)
        {
            var body = Encoding.UTF8.GetBytes(messageString);
            _channel.BasicPublish(exchange: "",
                            routingKey: queueName,
                            basicProperties: props,
                            body: body);
            return true;
        }
    }
}