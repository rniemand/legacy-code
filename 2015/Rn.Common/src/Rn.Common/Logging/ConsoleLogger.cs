using System;
using Rn.Common.Interfaces;

namespace Rn.Common.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }
    }
}
