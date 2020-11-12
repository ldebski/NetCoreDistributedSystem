using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sender.Services
{
    public interface IRabbitService
    {
        public IModel GetChannel();
        public string GetReplyQueueName();
    }
    public class RabbitService : IRabbitService
    {
        static private IConnection _conn;
        static private string replyQueueName;

        public RabbitService()
        {
            var _factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            _factory.UserName = "guest";
            _factory.Password = "guest";
            _conn = _factory.CreateConnection();

            replyQueueName = "reply_queue";
        }

        public IModel GetChannel()
        {
            return _conn.CreateModel();
        }

        public string GetReplyQueueName()
        {
            return replyQueueName;
        }
    }
}
