using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace receiverGet.Services
{
    public class DataBaseService
    {
        SqlConnection cnn;
        public DataBaseService()
        {
            string connectionString = @"Data Source=db;Initial Catalog=BankDataBase;User Id=sa; Password=STRONGpassword123!;MultipleActiveResultSets=True;";
            cnn = new SqlConnection(connectionString);
            cnn.Open();
            Console.WriteLine("Successfully connected to database!");
        }
        ~DataBaseService()
        {
            cnn.Close();
        }
        string CreateQuery(string message)
        {
            var tab = message.Split(".");
            string sql = $"IF (SELECT CashAmount FROM BankDataBase.dbo.Account WHERE AccountID = {tab[1]}) >= {tab[3]} " +
                         $"BEGIN" +
                         $"    UPDATE BankDataBase.dbo.Account SET CashAmount = CashAmount - {tab[3]} WHERE AccountID = {tab[1]};" +
                         $"    UPDATE BankDataBase.dbo.Account SET CashAmount = CashAmount + {tab[3]} WHERE AccountID = {tab[2]};" +
                         $"END";
            return sql;
        }

        public string Query(string message)
        {
            SqlCommand command = new SqlCommand(CreateQuery(message), cnn);
            command.ExecuteNonQuery();
            return "Transfer completed";
        }
    }
}
