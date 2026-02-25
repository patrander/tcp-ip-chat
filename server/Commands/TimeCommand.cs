using System;
using System.Threading.Tasks;

namespace server.Commands
{
    public class TimeCommand : IChatCommand
    {
        public string Name => "/time";
        public string Description => "Kiírja a szerver pontos idejét.";

        public Task<string> ExecuteAsync(string argument)
        {
            return Task.FromResult($"[Szerver Bot]: A szerver pontos ideje: {DateTime.Now:HH:mm:ss}");
        }
    }
}