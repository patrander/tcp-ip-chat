using System.Threading.Tasks;

namespace server.Commands
{
    public interface IChatCommand
    {
        string Name { get; } // A parancs neve (pl. "/time")
        string Description { get; } // Leírás a help menühöz
        Task<string> ExecuteAsync(string argument); // Maga a logika
    }
}