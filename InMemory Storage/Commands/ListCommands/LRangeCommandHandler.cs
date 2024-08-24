using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;

namespace InMemory_Storage.Commands.ListCommands
{
    public class LRangeCommandHandler : ICommandHandler
    {

        public LRangeCommandHandler(IListRepository storage)
        {
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        private readonly IListRepository Storage;

        public bool CanHandle(string commandType)
        {
            return commandType.Equals("LRANGE", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            var parts = command.Split(' ');

            if (parts.Length != 4)
            {
                throw new CommandFormatException(ErrorMessages.InvalidCommandFormatForLrange);
            }

            var key = parts[1];
            if (!int.TryParse(parts[2], out var start) || !int.TryParse(parts[3], out var stop))
            {
                return ErrorMessages.InvalidRangeFormat;
            }

            var items = await Task.Run(() => Storage.LRange(key, start, stop), cancellationToken);
            if (items.Any())
            {
                return string.Join(",", items);
            }

            return ResponseMessages.EmptyCode;
        }
    }
}
