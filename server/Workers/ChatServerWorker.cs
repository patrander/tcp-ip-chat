using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using server.Commands;
using server.Core;
using server.UI;

namespace server.Workers
{
    public class ChatServerWorker : BackgroundService
    {
        private readonly IChatServer _chatServer;
        private readonly ILogger<ChatServerWorker> _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        // 1. BEHOZTUK A KÖZPONTOT
        private readonly CommandDispatcher _dispatcher;

        // A konstruktorban elkérjük a DI konténertől a Dispatchert is
        public ChatServerWorker(
            IChatServer chatServer,
            ILogger<ChatServerWorker> logger,
            IHostApplicationLifetime appLifetime,
            CommandDispatcher dispatcher)
        {
            _chatServer = chatServer;
            _logger = logger;
            _appLifetime = appLifetime;
            _dispatcher = dispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConsoleUI.ShowBanner("TCP CHAT SERVER ENGINE");

            // 2. MÓDOSÍTOTTUK AZ ESEMÉNYT
            _chatServer.OnMessageReceived += message =>
            {
                if (message.StartsWith("/"))
                {
                    // Ha per jellel kezdődik, átadjuk a háttérben futó parancsfeldolgozónak
                    // A "tűz és felejtsd" (_ = ) módszerrel nem akasztjuk meg a szerver olvasását!
                    _ = HandleCommandAsync(message, stoppingToken);
                }
                else
                {
                    // Ha normál üzenet, csak simán kiírjuk, ahogy eddig
                    ConsoleUI.WritePartnerMessage("Kliens", message);
                }
            };

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

        // 3. ÚJ METÓDUS A PARANCSOK FUTTATÁSÁRA
        private async Task HandleCommandAsync(string message, CancellationToken token)
        {
            ConsoleUI.WriteSystemMessage($"Kliens parancsot kért: {message}");

            // A Központ (Dispatcher) megkeresi a megfelelő osztályt, és lefuttatja a logikát
            string response = await _dispatcher.ProcessAsync(message);

            // A kapott választ visszaküldjük a hálózaton a Kliensnek
            await _chatServer.SendMessageAsync(response, token);

            // Opcionális: A szerver ablakába is kiírjuk, hogy lássuk, mit dolgozott a bot
            ConsoleUI.WriteMyMessage(response);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _chatServer.Stop();
            return base.StopAsync(cancellationToken);
        }
    }
}