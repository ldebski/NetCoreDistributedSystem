using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using receiverGet.Services;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace receiver
{
    class Program
    {

        // static public string CreateQuery(string message)
        // {
        //     var tab = message.Split(".");
        //     var from = tab[0];
        //     var to = tab[1];
        //     var amount = tab[2];
        //     string sql = $"IF (SELECT CashAmount FROM BankDataBase.dbo.Account WHERE AccountID = {from}) >= {amount}" +
        //                  $"    UPDATE BankDataBase.dbo.Account SET CashAmount = CashAmount - {amount} WHERE AccountID = {from};" +
        //                  $"    UPDATE BankDataBase.dbo.Account SET CashAmount = CashAmount + {amount} WHERE AccountID = {to};";
        //     return sql;
        // }
        // 
        // static public void DataBaseConnection(string message)
        // {
        //     string connectionString = @"Data Source=db;Initial Catalog=BankDataBase;User Id=sa; Password=STRONGpassword123!;";
        //     SqlConnection cnn;
        //     cnn = new SqlConnection(connectionString);
        //     cnn.Open();
        //     Console.WriteLine("connected to database, executing query");
        //     SqlCommand command;
        //     command = new SqlCommand(CreateQuery(message), cnn);
        //     command.ExecuteNonQuery();
        //     cnn.Close();
        // }
        static void Main(string[] args)
        {
            Task.Delay(20000).Wait(); // waiting for rabbit & sql
            // creating connection to database
            DataBaseService dataBaseService = new DataBaseService();
            Console.WriteLine("Consuming Queue Now");

            ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: "przelew",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                // Console.WriteLine(" [x] Received from Rabbit: {0}", message);
                dataBaseService.Query(message);
                Console.WriteLine(" [x] Done processing message from Rabbit : {0}", message);
            };
            channel.BasicConsume(queue: "przelew",
                                    autoAck: true,
                                    consumer: consumer);

        }
    }
}
