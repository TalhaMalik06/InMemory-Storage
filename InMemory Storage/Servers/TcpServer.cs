using InMemory_Storage.Messages;
using InMemory_Storage.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
            CancellationTokenSource.Cancel();
            Listener.Stop();
            Logger.LogInformation("TCP listener {address} stopped at: {time}", Listener.LocalEndpoint, DateTimeOffset.Now);
        }

        public async void AcceptClientAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var client = await Listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(client, cancellationToken), cancellationToken);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ErrorMessages.ErrorAcceptingClient);
            }
            
        }

        public async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            try
            {
                var buffer = new byte[1024];
                var stream = client.GetStream();
                while (!cancellationToken.IsCancellationRequested)
                {
                    var byteCount = await stream.ReadAsync(buffer, cancellationToken);
                    if (byteCount <= 0 || cancellationToken.IsCancellationRequested) break;
                    var request = Encoding.UTF8.GetString(buffer, 0, byteCount).Trim();
                    var response = ProcessCommand(request);
                    var responseBytes = Encoding.UTF8.GetBytes(response + "\n");
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ErrorMessages.ErrorHandlingClient);
            }
            finally
            {
                client.Close();
                Logger.LogInformation(ErrorMessages.ClientClosedConnection);
            }
        }
        private string ProcessCommand(string request)
        {
            return $"Processed: {request}";
        }

    }
}
