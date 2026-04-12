using System;

namespace CybersecurityBot
{
    class Response
    {
        public string GetResponse(string input)
        {
            if (input.ToLower().Contains("how are you"))
            {
                return "I'm functioning smoothly, ready to help stay safe online! How can I help you?";
            }
            else if (input.ToLower().Contains("purpose") || input.ToLower().Contains("what can you do for me?"))
            {
                return "I'm here to educate you about cybersecurity threats like phishing, passwords, and unsafe browsing.";
            }
            else if (input.ToLower().Contains("what can i ask?"))
            {
                return "You can ask me about: phishing, password safety, safe browsing, and malware.";
            }
            else if (input.ToLower().Contains("phishing"))
            {
                return "Phishing is when attackers send fake emails/SMSs pretending to be from legitimate entities to steal personal data. Always check the emails/SMSs carefully and never click suspicious links.";
            }
            else if (input.ToLower().Contains("password"))
            {
                return "Use passwords at least 12 characters long with uppercase, lowercase, numbers and symbols. Never reuse passwords!";
            }
            else if (input.ToLower().Contains("browsing") || input.ToLower().Contains("safe browsing"))
            {
                return "Always look for HTTPS in the URL before entering personal information. Avoid clicking pop-ups and never download software from unknown websites.";
            }
            else if (input.ToLower().Contains("malware"))
            {
                return "Malware is malicious software designed to damage your device. Keep your antivirus updated and never open attachments from unknown senders.";
            }
            else if (input.ToLower().Contains("hello") || input.ToLower().Contains("hi"))
            {
                return "Hello! Great to chat with you. Ask me anything about cybersecurity!";
            }
            else if (input.ToLower().Contains("bye") || input.ToLower().Contains("exit") || input.ToLower().Contains("quit"))
            {
                return "EXIT";
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                return "It looks like you didn't type anything. Please ask me a cybersecurity question!";
            }
            else
            {
                return "I didn't quite understand that. Could you rephrase?";
            }
        }
    }
}