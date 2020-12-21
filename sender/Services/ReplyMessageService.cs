using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace sender.Services
{
    public interface IReplyMessageService
    {
        public void addObserver(string guid, ReplyObserver obsever);
    }

    public class ReplyMessageService : IReplyMessageService
    {
        private readonly IModel _channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        public ReplyObserverHandler observerHandler;

        public ReplyMessageService(IRabbitService rabbitService)
        {
            _channel = rabbitService.GetChannel();
            replyQueueName = rabbitService.GetReplyQueueName();

            _channel.QueueDeclare(replyQueueName,
                false,
                false,
                false,
                null);

            consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var tab = message.Split(".");
                observerHandler.SetObserver(tab[0], tab[1]);
            };

            _channel.BasicConsume(replyQueueName,
                true,
                consumer);

            observerHandler = new ReplyObserverHandler();

            Console.WriteLine("Created replies handler");
        }

        public void addObserver(string guid, ReplyObserver obsever)
        {
            observerHandler.Subscribe(guid, obsever);
        }
    }
}