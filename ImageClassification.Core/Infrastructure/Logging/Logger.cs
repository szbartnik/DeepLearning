using System;

namespace Wkiro.ImageClassification.Core.Infrastructure.Logging
{
    public class Logger : ILogger
    {
        public void LogWriteLine(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}