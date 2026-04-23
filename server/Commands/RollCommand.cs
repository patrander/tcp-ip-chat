using System;
using System.Threading.Tasks;

namespace server.Commands
{
    public class RollCommand : IChatCommand
    {
        public string Name => "/roll";
        public string Description => "Dob egy véletlen számot 1 és 100 között.";


        public Task<string> ExecuteAsync(string argument)
        {
            int roll = new Random().Next(1, 101);
            return Task.FromResult($"[Szerver Bot]: A kliens dobott egy {roll}-t! ");
        }
    }
}