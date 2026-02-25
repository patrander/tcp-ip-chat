using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

                    services.AddHostedService<ChatServerWorker>();
                    services.AddHostedService<ConsoleInputWorker>();
                })
                .Build();

            
            host.Run();
        }
    }
}