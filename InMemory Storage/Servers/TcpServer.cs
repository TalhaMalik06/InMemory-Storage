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

        }

        private readonly ILogger<TcpServer> Logger;
        private readonly TcpListener Listener;

        public void Start()
        {
            throw new NotImplementedException();
        }
        public void Stop()
        {
            throw new NotImplementedException();
        }

    }
}
