using System;
using System.Text;
using RabbitMQ.Client;

namespace KafkaConsumer
{
    public class MessageService
    {
        private readonly IModel _channel;
        private readonly IBasicProperties _props;
        private readonly string _replyQueueName;

        public MessageService()
        {
            _replyQueueName = "reply_queue";
            var factory = new ConnectionFactory {HostName = "rabbitmq", Port = 5672};
            factory.UserName = "guest";
            factory.Password = "guest";
            var conn = factory.CreateConnection();
            _channel = conn.CreateModel();

            _channel.QueueDeclare(_replyQueueName,
                false,
                false,
                false,
                null);

            _props = _channel.CreateBasicProperties();
            _props.ReplyTo = _replyQueueName;

            Console.WriteLine("Created messages handler");
        }

        public bool Enqueue(string messageString)
        {
            // Console.WriteLine("Enqueuing message: " + messageString);
            var body = Encoding.UTF8.GetBytes(messageString);
            _channel.BasicPublish("",
                _replyQueueName,
                _props,
                body);
            return true;
        }
    }
}