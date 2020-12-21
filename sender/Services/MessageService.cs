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
        private readonly IModel _channel;
        private readonly IBasicProperties props;
        private readonly string replyQueueName;

        public MessageService(IRabbitService rabbitService)
        {
            _channel = rabbitService.GetChannel();

            replyQueueName = rabbitService.GetReplyQueueName();
            _channel.QueueDeclare(replyQueueName,
                false,
                false,
                false,
                null);

            props = _channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;

            Console.WriteLine("Created messages handler");
        }

        public bool Enqueue(string messageString, string queueName)
        {
            var body = Encoding.UTF8.GetBytes(messageString);
            _channel.BasicPublish("",
                queueName,
                props,
                body);
            return true;
        }
    }
}