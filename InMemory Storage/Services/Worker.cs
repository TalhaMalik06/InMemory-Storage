using InMemory_Storage.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace InMemory_Storage.Services
{
    public class Worker : BackgroundService
    {
        public Worker(ILogger<Worker> logger, ITcpServer tcpServer)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            TcpServer = tcpServer ?? throw new ArgumentNullException(nameof(tcpServer));
        }

        private readonly ILogger<Worker> Logger;
        private readonly ITcpServer TcpServer;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            TcpServer.Start();
            while (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Worker active at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, cancellationToken);
            }
            TcpServer.Stop();
            Logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
        }
    }
}
