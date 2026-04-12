using System;


namespace CybersecurityBot
{
    internal class ConsoleUI
    {
        public static void DisplayASCIILogo()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("  ____      _                                        ");
            Console.WriteLine(" / ___|   _| |__   ___ _ __ ___  ___  ___           ");
            Console.WriteLine("| |  | | | | '_ \\ / _ \\ '__/ __|/ _ \\/ __|       ");
            Console.WriteLine("| |__| |_| | |_) |  __/ |  \\__ \\  __/ (__          ");
            Console.WriteLine(" \\____\\__, |_.__/ \\___|_|  |___/\\___|\\___|     ");
            Console.WriteLine("      |___/   Awareness Bot                         ");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.ResetColor();
        }
        
        public static void ShowBotMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[BOT]: ");
            Console.ResetColor();

            //Typing effect - print one character at a time
            foreach (char  c in message)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(18);
            }
            Console.WriteLine();
        }

        public static void PrintDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("----------------------------------------------------");
            Console.ResetColor();
        } 
          
        public static string GetUserInput(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"[{userName}]: ");
            Console.ResetColor();
            return Console.ReadLine();
        }
    }
}
