using System;

namespace server.UI 
{
    public static class ConsoleUI
    {
        
        private static readonly object _lock = new object();

        public static void ShowBanner(string title)
        {
            lock (_lock)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
___________.___________  ___________ ___________________          ____  __.________    _______ __________ 
\_   _____/|   \______ \ \_   _____//   _____/\____    /         |    |/ _|\______ \   \      \\______   \
 |    __)  |   ||    |  \ |    __)_ \_____  \   /     /   ______ |      <   |    |  \  /   |   \|     ___/
 |     \   |   ||    `   \|        \/        \ /     /_  /_____/ |    |  \  |    `   \/    |    \    |    
 \___  /   |___/_______  /_______  /_______  //_______ \         |____|__ \/_______  /\____|__  /____|    
     \/                \/        \/        \/         \/                 \/        \/         \/          
                ");
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
                Console.ForegroundColor = ConsoleColor.Magenta; // A partner színe lila
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
                Console.ForegroundColor = ConsoleColor.Green; // A te színed zöld
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