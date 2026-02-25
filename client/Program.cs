using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
              
                    services.Configure<ChatClientOptions>(hostContext.Configuration.GetSection("ChatClientSettings"));

                   
                    services.AddSingleton<IChatClient, ChatClient>();

                    
                    services.AddHostedService<ChatClientWorker>();
                    services.AddHostedService<ConsoleInputWorker>();
                })
                .Build();

            host.Run();
        }
    }
}