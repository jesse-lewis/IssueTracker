using IssueTracker;
using NUnit.Framework;
using System;

namespace Tests
{
    public class TestLogger : ILogger
    {
        public string LastError { get; set; }
        public string LastInfo { get; set; }
        public string LastWarning { get; set; }
        public string LastLog { get; set; }

        public void Error(string message)
        {
            LastError = message;
            Log(message);
        }

        public void Info(string message)
        {
            LastInfo = message;
            Log(message);
        }

        public void Warning(string message)
        {
            LastWarning = message;
            Log(message);
        }

        private void Log(string message)
        {
            LastLog = message;
        }
    }
}