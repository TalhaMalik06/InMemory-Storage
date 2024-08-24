using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;

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

        public async Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            var parts = command.Split(' ');
            if (parts.Length < 4 || !int.TryParse(parts[2], out var ttl))
            {
                throw new CommandFormatException(ErrorMessages.InvalidCommandFormatForSetWithExpiry);
            }

            var key = parts[1];
            var value = string.Join(' ', parts.Skip(3));
            await Task.Run(() => KeyValueRepository.Set(key, value, TimeSpan.FromSeconds(ttl)), cancellationToken);

            return ResponseMessages.SuccessCode;
        }
    }
}
