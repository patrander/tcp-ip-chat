using System;
using System.Threading;
using System.Threading.Tasks;

namespace client
{
    public interface IChatClient
    {
        event Action<string> OnMessageReceived;
        event Action OnDisconnected;

        Task ConnectAsync(CancellationToken cancellationToken);
        Task SendMessageAsync(string message, CancellationToken cancellationToken);
        void Disconnect();
    }
}