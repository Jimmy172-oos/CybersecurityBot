using System;
using System.Collections.Generic;

namespace CybersecurityBot
{
    class RandomResponses
    {
        private Random random = new Random();

        // Lists of multiple responses for each topic
        private List<string> phishingTips = new List<string>()
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
            "Always check the sender's email address carefully. Fake emails often use addresses that look almost correct but have small differences.",
            "Never click on suspicious links in emails or SMSs. When in doubt, go directly to the official website instead.",
            "Legitimate organisations will never ask for your password or banking details via email."
        };

        private List<string> passwordTips = new List<string>()
        {
            "Use a password that is at least 12 characters long with uppercase, lowercase, numbers and symbols.",
            "Never use the same password for multiple accounts. If one gets hacked, all your accounts become vulnerable.",
            "Consider using a password manager to generate and store strong passwords safely.",
            "Avoid using personal details like your name, birthday or pet's name in your passwords."
        };

        private List<string> scamTips = new List<string>()
        {
            "If something sounds too good to be true online, it probably is. Be very careful of prize notifications.",
            "Never send money to someone you haven't met in person, even if they seem legitimate online.",
            "Be wary of urgent messages pressuring you to act immediately — this is a common scam tactic.",
            "Always verify requests for personal or financial information by contacting the organisation directly."
        };

        private List<string> privacyTips = new List<string>()
        {
            "Review your social media privacy settings regularly and limit who can see your personal information.",
            "Be careful about what personal information you share online — scammers can use it against you.",
            "Use two-factor authentication on all your important accounts for an extra layer of security.",
            "Regularly check what apps have access to your personal data and remove ones you no longer use."
        };

        public string GetRandomPhishingTip()
        {
            int index = random.Next(phishingTips.Count);
            return phishingTips[index];
        }

        public string GetRandomPasswordTip()
        {
            int index = random.Next(passwordTips.Count);
            return passwordTips[index];
        }

        public string GetRandomScamTip()
        {
            int index = random.Next(scamTips.Count);
            return scamTips[index];
        }

        public string GetRandomPrivacyTip()
        {
            int index = random.Next(privacyTips.Count);
            return privacyTips[index];
        }
    }
}