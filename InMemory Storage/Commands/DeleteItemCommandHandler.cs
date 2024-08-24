using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;

namespace InMemory_Storage.Commands
{
    public class DeleteItemCommandHandler : ICommandHandler
    {

        public DeleteItemCommandHandler(IKeyValueRepository keyValueRepository) 
        {
            KeyValueRepository = keyValueRepository ?? throw new ArgumentNullException(nameof(keyValueRepository));
        }

        private IKeyValueRepository KeyValueRepository { get; }
        public bool CanHandle(string commandType)
        {
            return commandType.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            var parts = command.Split(' ');
            if (parts.Length < 2)
            {
                throw new CommandFormatException(ErrorMessages.InvalidCommandFormatForDelete);
            }

            var key = parts[1];
            await Task.Run(() => KeyValueRepository.Delete(key), cancellationToken);

            return ResponseMessages.SuccessCode;

        }
    }
}
