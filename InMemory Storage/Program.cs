using InMemory_Storage.Commands;
using InMemory_Storage.Commands.ListCommands;
using InMemory_Storage.Models;
using InMemory_Storage.Repository;
using InMemory_Storage.Server;
using InMemory_Storage.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InMemory_Storage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<TcpServerSettings>(hostContext.Configuration.GetSection("TcpServerSettings"));
                    services.AddHostedService<Worker>();
                    services.AddScoped<ITcpServer, TcpServer>();
                    services.AddScoped<ICommandFactory, CommandHandlerFactory>();
                    services.AddScoped<ICommandHandler, SetItemCommandHandler>();
                    services.AddScoped<ICommandHandler, SetWithExpiryCommandHandler>();
                    services.AddScoped<ICommandHandler, GetItemCommandHandler>();
                    services.AddScoped<ICommandHandler, DeleteItemCommandHandler>();
                    services.AddScoped<ICommandHandler, LPushCommandHandler>();
                    services.AddScoped<ICommandHandler, RPushCommandHandler>();
                    services.AddScoped<ICommandHandler, LPopCommandHandler>();
                    services.AddScoped<ICommandHandler, RPopCommandHandler>();
                    services.AddScoped<ICommandHandler, LRangeCommandHandler>();
                    services.AddScoped<ICommandHandler, LLenCommandHandler>();
                    services.AddSingleton<IKeyValueRepository, KeyValueRepository>();
                    services.AddSingleton<IListRepository, ListRepository>();
                });
    }
}