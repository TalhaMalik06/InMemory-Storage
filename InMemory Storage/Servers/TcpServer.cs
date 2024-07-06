using InMemory_Storage.Messages;
using InMemory_Storage.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;

namespace InMemory_Storage.Server
{
    public class TcpServer : ITcpServer
    {
        public TcpServer(ILogger<TcpServer> logger, IOptions<TcpServerSettings> settings)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (settings?.Value == null || string.IsNullOrWhiteSpace(settings.Value.Address))
            {
                throw new ArgumentNullException(nameof(settings), ErrorMessages.TcpSettingNull);
            }

            if (settings.Value.Port <= 0 || settings.Value.Port > 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(settings.Value.Port), ErrorMessages.TcpPortInvalid);
            }

            Listener = new TcpListener(IPAddress.Parse(settings.Value.Address), settings.Value.Port);
            CancellationTokenSource = new CancellationTokenSource();

        }

        private readonly ILogger<TcpServer> Logger;
        private readonly TcpListener Listener;
        private readonly CancellationTokenSource CancellationTokenSource;

        public void Start()
        {
            Listener.Start();
            Logger.LogInformation("TCP listener {address} started at: {time}", Listener.LocalEndpoint ,DateTimeOffset.Now);
            AcceptClientAsync(CancellationTokenSource.Token);
        }
        public void Stop()
        {
            Listener.Stop();
            CancellationTokenSource.Cancel();
            Logger.LogInformation("TCP listener {address} stopped at: {time}", Listener.LocalEndpoint, DateTimeOffset.Now);
        }

        public async void AcceptClientAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                using var client = await Listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client, cancellationToken));
            }
        }

        public async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
