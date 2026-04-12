using System;

namespace CybersecurityBot
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI.DisplayASCIILogo();
            ChatBot bot = new ChatBot();
            bot.PlayVoiceGreeting();
            bot.Start();
        }
    }
}