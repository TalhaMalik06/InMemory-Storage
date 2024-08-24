using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;
using KeyNotFoundException = InMemory_Storage.Exceptions.KeyNotFoundException;

namespace InMemory_Storage.Commands.ListCommands
{
    public class LPopCommandHandler : ICommandHandler
    {
        public LPopCommandHandler(IListRepository storage)
        {
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        private readonly IListRepository Storage;

        public bool CanHandle(string commandType)
        {
            return commandType.Equals("LPOP", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            var parts = command.Split(' ');

            if (parts.Length != 2)
            {
                throw new CommandFormatException(ErrorMessages.InvalidCommandFormatForLpop);
            }

            var key = parts[1];
            var result = await Task.Run(() => Storage.LPop(key), cancellationToken);

            return result ?? throw new KeyNotFoundException(ErrorMessages.ListEmptyOrDoesNotExists);
        }
    }
}
