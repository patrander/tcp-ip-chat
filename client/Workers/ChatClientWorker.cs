using System;
using System.Threading;
using System.Threading.Tasks;
using client.Core;
using client.UI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace client.Workers
{
    public class ChatClientWorker : BackgroundService
    {
        private readonly IChatClient _chatClient;
        private readonly ILogger<ChatClientWorker> _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        public ChatClientWorker(IChatClient chatClient, ILogger<ChatClientWorker> logger, IHostApplicationLifetime appLifetime)
        {
            _chatClient = chatClient;
            _logger = logger;
            _appLifetime = appLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            ConsoleUI.ShowBanner("TCP CHAT KLIENS");

            
            _chatClient.OnMessageReceived += message =>
                ConsoleUI.WritePartnerMessage("Szerver", message);

            _chatClient.OnDisconnected += () =>
            {
                ConsoleUI.WriteError("A szerver lecsatlakozott. Nyomj Entert a kilépéshez!");
                _appLifetime.StopApplication();
            };

            try
            {
                ConsoleUI.WriteSystemMessage("Kapcsolódás a szerverhez...");
                await _chatClient.ConnectAsync(stoppingToken);
                ConsoleUI.WriteSystemMessage("Sikeresen csatlakozva a beszélgetéshez!");
            }
            catch
            {
                ConsoleUI.WriteError("Nem sikerült elérni a szervert.");
                _appLifetime.StopApplication();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _chatClient.Disconnect();
            return base.StopAsync(cancellationToken);
        }



    }
}