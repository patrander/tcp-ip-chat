using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace server
{
    public class ConsoleInputWorker : BackgroundService
    {
        private readonly IChatServer _chatServer;
        private readonly IHostApplicationLifetime _appLifetime;

        public ConsoleInputWorker(IChatServer chatServer, IHostApplicationLifetime appLifetime)
        {
            _chatServer = chatServer;
            _appLifetime = appLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000, stoppingToken);

            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input)) continue;

                    if (input.Trim().ToLower() == "exit")
                    {
                        ConsoleUI.WriteSystemMessage("Szerver leállítása...");
                        _appLifetime.StopApplication();
                        break;
                    }

                    
                    await _chatServer.SendMessageAsync(input, stoppingToken);

                    
                    ConsoleUI.WriteMyMessage(input);
                }
            }, stoppingToken);
        }
    }
}