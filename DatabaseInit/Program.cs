using System;
using System.Threading.Tasks;
using kafka;
using receiverGet.Services;

namespace DatabaseInit
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task.Delay(30000).Wait(); // waiting for sql and kafka
            var kafkaProducerHostedService = new KafkaProducerHostedService();
            var dataBaseService = new DataBaseService();
            for (var i = 1; i < 1000; i++)
            {
                var amount = dataBaseService.Query(i.ToString());
                var amountInt = new int();
                if (amount != null && int.TryParse(amount, out amountInt))
                {
                    Console.WriteLine("Writing to kafka");
                    Console.WriteLine(amount);
                    kafkaProducerHostedService.StartAsync(amountInt, "", i.ToString(), "0");
                }
                else
                {
                    Console.WriteLine($"Failed to init database with id {i}, amount: " + amount);
                }
            }
        }
    }
}