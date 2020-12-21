namespace KafkaConsumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kafkaConsumerHostedService = new KafkaConsumerHostedService();
            kafkaConsumerHostedService.StartAsync();
        }
    }
}