using System;

namespace ImageClassification.Infrastructure
{
    public class Logger : ILogger
    {
        public void WriteLine(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}