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

        public string Query(string message)
        {
            SqlDataReader dataReader;
            String query, Output = "";
            query = $"SELECT * FROM BankDataBase.dbo.Account WHERE AccountID = {message};";
            SqlCommand command = new SqlCommand(query, cnn);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                // Output = $"Id: {dataReader.GetValue(0)} Amount of money: {dataReader.GetValue(1)}";
                Output = $"{dataReader.GetValue(1)}";
            }
            return Output;
        }
    }
}
