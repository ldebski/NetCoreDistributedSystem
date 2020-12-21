using RabbitMQ.Client;

namespace sender.Services
{
    public interface IRabbitService
    {
        public IModel GetChannel();
        public string GetReplyQueueName();
    }

    public class RabbitService : IRabbitService
    {
        private static IConnection _conn;
        private static string replyQueueName;

        public RabbitService()
        {
            var _factory = new ConnectionFactory {HostName = "rabbitmq", Port = 5672};
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