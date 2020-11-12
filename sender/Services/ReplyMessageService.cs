﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sender.Services
{
    public interface IReplyMessageService
    {
        public ConcurrentDictionary<string, string> GetReplyDictionary();
    }
    public class ReplyMessageService: IReplyMessageService
    {
        readonly IModel _channel;
        private readonly string replyQueueName;
        private EventingBasicConsumer consumer;
        public ConcurrentDictionary<string, string> replyDict;

        public ReplyMessageService(IRabbitService rabbitService)
        {
            _channel = rabbitService.GetChannel();
            replyQueueName = rabbitService.GetReplyQueueName();

            _channel.QueueDeclare(queue: replyQueueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                Console.WriteLine("got message: " + Encoding.UTF8.GetString(body));
                var message = Encoding.UTF8.GetString(body);
                var tab = message.Split(".");
                replyDict.TryAdd(tab[0], tab[1]);
            };

            _channel.BasicConsume(queue: replyQueueName,
                                    autoAck: true,
                                    consumer: consumer);

            Console.WriteLine("Created replies handler");
        }

        public ConcurrentDictionary<string, string> GetReplyDictionary()
        {
            return replyDict;
        }
    }
}