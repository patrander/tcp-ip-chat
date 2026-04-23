using System;

namespace client.UI 
{
    public static class ConsoleUI
    {
        
        private static readonly object _lock = new object();

        public static void ShowBanner(string title)
        {
            lock (_lock)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(@"

_________ .____    .______________ __________________
\_   ___ \|    |   |   \_   _____/ \      \__    ___/
/    \  \/|    |   |   ||    __)_  /   |   \|    |   
\     \___|    |___|   ||        \/    |    \    |   
 \______  /_______ \___/_______  /\____|__  /____|   
        \/        \/           \/         \/               ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(new string('=', 90));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  {title} - (Kilépés: 'exit')");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(new string('=', 90) + "\n");
            }
        }

        public static void WriteSystemMessage(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [RENDSZER] {message}");
                Console.ResetColor();
            }
        }

        public static void WritePartnerMessage(string partnerName, string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.Magenta; 
                Console.Write($"[{partnerName}]: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public static void WriteMyMessage(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.Green; 
                Console.Write("[Én]: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public static void WriteError(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[HIBA] {message}");
                Console.ResetColor();
            }
        }
    }
}