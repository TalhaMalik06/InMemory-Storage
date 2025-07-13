using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;

namespace InMemory_Storage.Commands
{
    public class SetItemCommandHandler : ICommandHandler
    {
        public SetItemCommandHandler(IKeyValueRepository keyValueRepository)
        {
            KeyValueRepository = keyValueRepository ?? throw new ArgumentNullException(nameof(keyValueRepository));
        }

        private IKeyValueRepository KeyValueRepository { get; }
        public bool CanHandle(string commandType)
        {
            return commandType.Equals("SET", StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            var parts = command.Split(' ');
            if (parts.Length < 3)
            {
                throw new CommandFormatException(ErrorMessages.InvalidCommandFormatForSet);
            }

            var key = parts[1];
            var value = parts[2];
            KeyValueRepository.Set(key, value);

            return Task.FromResult(ResponseMessages.SuccessCode);
        }
    }
}
