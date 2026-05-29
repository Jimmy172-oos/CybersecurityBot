using System;

namespace CybersecurityBot
{
    class SentimentDetector
    {
        public string DetectSentiment(string input)
        {
            string lowered = input.ToLower();

            if (lowered.Contains("worried") || lowered.Contains("scared") || lowered.Contains("nervous"))
            {
                return "worried";
            }
            else if (lowered.Contains("frustrated") || lowered.Contains("angry") || lowered.Contains("annoyed"))
            {
                return "frustrated";
            }
            else if (lowered.Contains("curious") || lowered.Contains("interested") || lowered.Contains("want to know"))
            {
                return "curious";
            }
            else if (lowered.Contains("confused") || lowered.Contains("don't understand") || lowered.Contains("not sure"))
            {
                return "confused";
            }
            else
            {
                return "neutral";
            }
        }

        public string GetSentimentResponse(string sentiment)
        {
            if (sentiment == "worried")
            {
                return "It's completely understandable to feel that way. You're not alone — many people worry about online safety. Let me share some tips to help you stay protected.";
            }
            else if (sentiment == "frustrated")
            {
                return "I understand your frustration. Cybersecurity can feel overwhelming at times. Let's take it step by step — I'm here to help.";
            }
            else if (sentiment == "curious")
            {
                return "I love the curiosity! Wanting to learn more about cybersecurity is the first step to staying safe online.";
            }
            else if (sentiment == "confused")
            {
                return "No worries at all — cybersecurity can be confusing. Let me try to explain things more clearly for you.";
            }
            else
            {
                return "";
            }
        }
    }
}