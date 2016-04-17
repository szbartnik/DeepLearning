using System;

namespace ImageClassification.Infrastructure.Logging
{
    public class Logger : ILogger
    {
        public void WriteLine(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}