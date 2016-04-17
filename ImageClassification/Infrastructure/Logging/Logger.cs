using System;

namespace Wkiro.ImageClassification.Infrastructure.Logging
{
    public class Logger : ILogger
    {
        public void WriteLine(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}