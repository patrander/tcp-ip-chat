using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace client
{
    public class ConsoleInputWorker : BackgroundService
    {
        private readonly IChatClient _chatClient;
        private readonly IHostApplicationLifetime _appLifetime;

        public ConsoleInputWorker(IChatClient chatClient, IHostApplicationLifetime appLifetime)
        {
            _chatClient = chatClient;
            _appLifetime = appLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(500, stoppingToken);

            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    string input = Console.ReadLine();

                    
                    if (string.IsNullOrWhiteSpace(input)) continue;

                    if (input.Trim().ToLower() == "exit")
                    {
                        ConsoleUI.WriteSystemMessage("Kilépés iniciálva...");
                        _appLifetime.StopApplication();
                        break;
                    }

                    
                    await _chatClient.SendMessageAsync(input, stoppingToken);

                    
                    ConsoleUI.WriteMyMessage(input);
                }
            }, stoppingToken);
        }
    }
}