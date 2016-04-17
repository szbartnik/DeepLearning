using System;

namespace Polsl.Inf.Os2.WKiRO.ImageClassification.Infrastructure.Logging
{
    public class Logger : ILogger
    {
        public void WriteLine(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}