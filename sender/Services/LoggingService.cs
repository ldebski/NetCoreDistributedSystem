using System;
using System.IO;

namespace sender.Services 
{
    public class LoggingService
    {
        private string fileName {get;set;}

        private Guid guid {get;set;}

        public LoggingService() 
        {
            guid = Guid.NewGuid();

            fileName = "/app/logs/sender.log";

            using (File.Create(fileName));
        }

        // Writing message into log file
        // Pattern:
        // [GUID][DateTime.Now] Message (/n)
        public void toFile(string text) 
        {   
            File.AppendAllText(fileName, "[" + guid.ToString() + "]" + 
                                         "[" + DateTime.Now.ToString() + "] " 
                                             + text + "\n");
        }
    }
}