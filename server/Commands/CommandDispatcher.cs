using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Commands
{
    public class CommandDispatcher
    {
        private readonly Dictionary<string, IChatCommand> _commands;

        // A Host automatikusan beadja ide a listát az összes regisztrált parancsból!
        public CommandDispatcher(IEnumerable<IChatCommand> commands)
        {
            // Egy gyors szótárba (Dictionary) tesszük őket a nevük alapján a villámgyors kereséshez
            _commands = commands.ToDictionary(c => c.Name, c => c);
        }

        public async Task<string> ProcessAsync(string input)
        {
            string[] parts = input.Trim().Split(' ', 2); // Levágjuk az esetleges paramétereket (pl. "/roll 20")
            string commandName = parts[0].ToLower();
            string argument = parts.Length > 1 ? parts[1] : string.Empty;

            // Beépített /help parancs, ami dinamikusan generálódik a meglévő parancsokból
            if (commandName == "/help")
            {
                var helpText = "[Szerver Bot]: Elérhető parancsok:\n";
                foreach (var cmd in _commands.Values)
                {
                    helpText += $"  {cmd.Name} - {cmd.Description}\n";
                }
                helpText += "  /help - Listázza a parancsokat.";
                return helpText;
            }

            // Ha létezik a parancs a szótárunkban, futtatjuk!
            if (_commands.TryGetValue(commandName, out var command))
            {
                return await command.ExecuteAsync(argument);
            }

            return $"[Szerver Bot]: Ismeretlen parancs: '{commandName}'. Írd be, hogy /help";
        }
    }
}