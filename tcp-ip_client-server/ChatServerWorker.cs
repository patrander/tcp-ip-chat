using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace server
{
    public class ChatServerWorker : BackgroundService
    {
        private readonly IChatServer _chatServer;
        private readonly ILogger<ChatServerWorker> _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        public ChatServerWorker(IChatServer chatServer, ILogger<ChatServerWorker> logger, IHostApplicationLifetime appLifetime)
        {
            _chatServer = chatServer;
            _logger = logger;
            _appLifetime = appLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            ConsoleUI.ShowBanner("TCP CHAT SERVER ENGINE");

            
            _chatServer.OnMessageReceived += message =>
                ConsoleUI.WritePartnerMessage("Kliens", message);

            _chatServer.OnClientDisconnected += () =>
            {
                ConsoleUI.WriteError("A kliens lecsatlakozott. Várakozás új kapcsolatra vagy leállás...");
              
            };

            try
            {
                await _chatServer.StartAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                ConsoleUI.WriteError($"Súlyos hiba történt: {ex.Message}");
                _appLifetime.StopApplication();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _chatServer.Stop();
            return base.StopAsync(cancellationToken);
        }
    }
}