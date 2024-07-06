using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Services
{
    public class Worker : BackgroundService
    {
        public Worker(ILogger<Worker> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private readonly ILogger<Worker> Logger;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            return Task.CompletedTask;
        }
    }
}
