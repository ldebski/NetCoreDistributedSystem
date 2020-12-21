using System;
using System.Data.SqlClient;

namespace receiverGet.Services
{
    public class DataBaseService
    {
        private readonly SqlConnection cnn;

        public DataBaseService()
        {
            var connectionString =
                @"Data Source=accounts_db;Initial Catalog=BankDataBase;User Id=sa; Password=STRONGpassword123!;MultipleActiveResultSets=True;";
            cnn = new SqlConnection(connectionString);
            cnn.Open();
            Console.WriteLine("Successfully connected to database!");
        }

        ~DataBaseService()
        {
            cnn.Close();
        }

        private string CreateQuery(string message)
        {
            // Console.WriteLine(message);
            var tab = message.Split(".");
            var sql = $"IF (SELECT CashAmount FROM BankDataBase.dbo.Account WHERE AccountID = {tab[1]}) >= {tab[3]} " +
                      "BEGIN" +
                      $"    UPDATE BankDataBase.dbo.Account SET CashAmount = CashAmount - {tab[3]} WHERE AccountID = {tab[1]};" +
                      $"    UPDATE BankDataBase.dbo.Account SET CashAmount = CashAmount + {tab[3]} WHERE AccountID = {tab[2]};" +
                      "END";

            // weź stan tymczasowy wszystkie dotychczasowe żądania z tego konta sprawdź czy możesz 
            return sql;
        }

        // 1 100
        // 2 100
        // 3 0
        // 1->2 100
        // 1->3 100 ? weźmie ostatni stan konta 1 = 100, weźmie wszystkie żądania które zostały zlecone z tymczasowej bazy danych 1->2 100
        // zapis do bazy danych po paru godzinach, sesja eliksir
        // 1 0
        // 2 200
        // 3 0
        // 2->3 200
        // zapis do bazy danych po paru godzinach, sesja eliksir

        public string Query(string message)
        {
            var command = new SqlCommand(CreateQuery(message), cnn);
            command.ExecuteNonQuery();
            return "Transfer completed";
        }
    }
}