using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server.Configuration;

namespace server.Core
{
    public class ChatServer : IChatServer
    {
        public event Action<string> OnMessageReceived;
        public event Action OnClientDisconnected;

        private readonly ILogger<ChatServer> _logger;
        private readonly ChatServerOptions _options;

        private TcpListener _listener;
        private TcpClient _client;
        private NetworkStream _stream;

        
        public ChatServer(ILogger<ChatServer> logger, IOptions<ChatServerOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var ip = IPAddress.Parse(_options.IpAddress);
                _listener = new TcpListener(ip, _options.Port);
                _listener.Start();

                
                _logger.LogInformation("Szerver elindult a {IP}:{Port} címen.", _options.IpAddress, _options.Port);
                _logger.LogInformation("Várakozás a kliens csatlakozására...");

                _client = await _listener.AcceptTcpClientAsync(cancellationToken);
                _stream = _client.GetStream();

                _logger.LogInformation("Kliens sikeresen csatlakozott!");

                
                _ = Task.Run(() => ReadLoopAsync(cancellationToken), cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Hiba a szerver indításakor.");
                Stop();
            }
        }

        public async Task SendMessageAsync(string message, CancellationToken cancellationToken)
        {
            if (_stream == null || !_client.Connected) return;

            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
            }
            catch (Exception ex) when (ex is IOException || ex is SocketException)
            {
                _logger.LogWarning("A kapcsolat megszakadt üzenetküldés közben.");
            }
        }

        private async Task ReadLoopAsync(CancellationToken token)
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (!token.IsCancellationRequested)
                {
                    int byteCount = await _stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (byteCount == 0)
                    {
                        _logger.LogInformation("A kliens váratlanul bontotta a kapcsolatot.");
                        OnClientDisconnected?.Invoke();
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    OnMessageReceived?.Invoke(message);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is SocketException || ex is OperationCanceledException)
            {
                _logger.LogInformation("Az olvasási folyamat befejeződött.");
                OnClientDisconnected?.Invoke();
            }
        }

        public void Stop()
        {
            _stream?.Close();
            _client?.Close();
            _listener?.Stop();
            _logger.LogInformation("Szerver hálózati erőforrások felszabadítva.");
        }
    }
}