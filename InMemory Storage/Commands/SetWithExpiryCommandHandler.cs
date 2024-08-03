using InMemory_Storage.Messages;
using InMemory_Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Commands
{
    public class SetWithExpiryCommandHandler : ICommandHandler
    {
        public SetWithExpiryCommandHandler(IKeyValueRepository keyValueRepository)
        {
            KeyValueRepository = keyValueRepository ?? throw new ArgumentNullException(nameof(keyValueRepository));
        }
        private IKeyValueRepository KeyValueRepository { get; }

        public bool CanHandle(string commandType)
        {
            return commandType.Equals("SETEX", StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            var parts = command.Split(' ');
            if (parts.Length < 4 || !int.TryParse(parts[2], out var ttl))
            {
                return Task.FromResult(ErrorMessages.InvalidCommandFormatForSetWithExpiry);
            }

            var key = parts[1];
            var value = string.Join(' ', parts.Skip(3));
            KeyValueRepository.Set(key, value, TimeSpan.FromSeconds(ttl));

            return Task.FromResult("OK");
        }
    }
}
