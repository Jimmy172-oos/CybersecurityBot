using System;
using System.IO;
using System.Diagnostics;

namespace CybersecurityBot
{
    class ChatBot
    {
        private string userName;
        private Response response;

        public ChatBot()
        {
            response = new Response();
        }

        public void PlayVoiceGreeting()
      
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav.wav");

                if (File.Exists(audioPath))
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "powershell";
                    psi.Arguments = $"-c (New-Object Media.SoundPlayer '{audioPath}').PlaySync()";
                    psi.UseShellExecute = false;
                    psi.CreateNoWindow = true; // hides any popup window

                    System.Threading.Thread.Sleep(500); // small delay before playing
                    Process audioProcess = Process.Start(psi);
                    audioProcess.WaitForExit();
                    System.Threading.Thread.Sleep(500); // small delay after playing
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("[INFO] Voice greeting file not found. Skipping audio.");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Could not play greeting: " + ex.Message);
            }
        }

        public void Start()
        {
            ConsoleUI.PrintDivider();
            ConsoleUI.ShowBotMessage("Before we get started, what is your name?");
            ConsoleUI.PrintDivider();

            Console.Write("Your name: ");
            userName = Console.ReadLine();

            // Validate input. Keeps asking until a name is given
            while (string.IsNullOrWhiteSpace(userName))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("[!] Name cannot be empty. Please enter your name: ");
                Console.ResetColor();
                Console.Write("Your name: ");
                userName = Console.ReadLine();
            }

            ConsoleUI.PrintDivider();
            ConsoleUI.ShowBotMessage($"Hello, {userName}! I'm a Cybersecurity Awareness Assistant.");
            ConsoleUI.ShowBotMessage("You can ask me about any cybersecurity related topics, such as phishing, password safety, safe browsing, and malware.");
            ConsoleUI.ShowBotMessage("Type 'exit', 'quit' or 'bye' to leave.");
            ConsoleUI.PrintDivider();

            // Conversation loop
            while (true)
            {
                string userInput = ConsoleUI.GetUserInput(userName);
                string botResponse = response.GetResponse(userInput);

                ConsoleUI.PrintDivider();

                // Show thinking effect before responding
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[BOT is thinking]");
                System.Threading.Thread.Sleep(1500); // waits 1.5 seconds
                Console.Write("\r                          \r"); // clears the thinking line
                Console.ResetColor();

                if (botResponse == "EXIT")
                {
                    ConsoleUI.ShowBotMessage($"Be careful online and stay safe, {userName}! Goodbye!");
                    ConsoleUI.PrintDivider();
                    break;
                }

                ConsoleUI.ShowBotMessage(botResponse);
                ConsoleUI.PrintDivider();
            }
        }
    }
}
