using System;

namespace CybersecurityBot
{
    class Response
    {
        private RandomResponses randomResponses = new RandomResponses();
        public string lastTopic = "";

        public string GetResponse(string input)
        {
            string lower = input.ToLower();

            // PHISHING
            if (lower.Contains("phishing"))
            {
                lastTopic = "phishing";
                return randomResponses.GetRandomPhishingTip();
            }

            // PASSWORD
            if (lower.Contains("password"))
            {
                lastTopic = "password";
                return randomResponses.GetRandomPasswordTip();
            }

            // SCAM
            if (lower.Contains("scam"))
            {
                lastTopic = "scam";
                return randomResponses.GetRandomScamTip();
            }

            // PRIVACY
            if (lower.Contains("privacy"))
            {
                lastTopic = "privacy";
                return randomResponses.GetRandomPrivacyTip();
            }

            // MALWARE
            if (lower.Contains("malware") || lower.Contains("virus"))
            {
                lastTopic = "malware";
                return "Malware is malicious software. Keep your antivirus updated!";
            }

            // BROWSING
            if (lower.Contains("browsing") || lower.Contains("safe browsing"))
            {
                lastTopic = "browsing";
                return "Look for HTTPS in URLs. Avoid suspicious downloads.";
            }

            // GREETINGS
            if (lower.Contains("hello") || lower.Contains("hi") || lower.Contains("hey"))
            {
                return "Hello! How can I help you with cybersecurity today?";
            }

            // HOW ARE YOU
            if (lower.Contains("how are you"))
            {
                return "I'm doing great! Ready to help you stay safe online!";
            }

            // THANK YOU
            if (lower.Contains("thank"))
            {
                return "You're welcome! Stay safe online!";
            }

            // EXIT
            if (lower.Contains("bye") || lower.Contains("exit") || lower.Contains("quit"))
            {
                return "EXIT";
            }

            // DEFAULT
            return "I can help with cybersecurity topics like phishing, passwords, scams, and privacy. Type 'help' to see all commands!";
        }
    }
}