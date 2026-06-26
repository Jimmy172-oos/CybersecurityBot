using System;
using System.Collections.Generic;

namespace CybersecurityBot
{
    public class ActivityLog
    {
        private List<string> logEntries = new List<string>();

        public void AddEntry(string action)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            logEntries.Add($"[{timestamp}] {action}");
        }

        public string GetFormattedLog()
        {
            if (logEntries.Count == 0)
                return "No actions recorded yet.";

            int start = Math.Max(0, logEntries.Count - 10);
            string log = "";
            int count = 1;
            for (int i = start; i < logEntries.Count; i++)
            {
                log += count + ". " + logEntries[i] + "\n";
                count++;
            }
            return log;
        }
    }
}