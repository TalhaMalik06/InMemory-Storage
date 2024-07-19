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

        public Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            var parts = command.Split(' ');
            if (parts.Length < 2)
            {
                return Task.FromResult(ErrorMessages.InvalidCommandFormatForDelete);
            }

            var key = parts[1];

            KeyValueRepository.DeleteItem(key);

            return Task.FromResult("OK");


        }
    }
}
