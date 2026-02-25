using System;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    public interface IChatServer
    {
        event Action<string> OnMessageReceived;
        event Action OnClientDisconnected;
    

        Task StartAsync(CancellationToken cancellationToken);
        Task SendMessageAsync(string message, CancellationToken cancellationToken);
        void Stop();
    }
}