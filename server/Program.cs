using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using server.Commands;
using server.Configuration;
using server.Core;
using server.Workers;

namespace server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                   
                    services.Configure<ChatServerOptions>(hostContext.Configuration.GetSection("ChatServerSettings"));
                    
                    services.AddSingleton<IChatServer, ChatServer>();

                    services.AddSingleton<CommandDispatcher>();

                    services.AddTransient<IChatCommand, TimeCommand>();
                    services.AddTransient<IChatCommand, RollCommand>();

                    services.AddHostedService<ChatServerWorker>();
                    services.AddHostedService<ConsoleInputWorker>();
                })
                .Build();

            
            host.Run();
        }
    }
}