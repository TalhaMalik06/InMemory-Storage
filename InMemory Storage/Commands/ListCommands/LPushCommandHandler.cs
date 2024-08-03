using InMemory_Storage.Messages;
using InMemory_Storage.Repository;
using InMemory_Storage.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Commands.ListCommands
{
    public class LPushCommandHandler : ICommandHandler
    {
        public LPushCommandHandler(IListRepository storage)
        {
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        private readonly IListRepository Storage;
        public bool CanHandle(string commandType)
        {
            return commandType.Equals("LPUSH", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            var parts = command.Split(' ');

            if (parts.Length < 3)
            {
                return await Task.FromResult(ErrorMessages.InvalidCommandFormatForLpush);
            }
            var key = parts[1];
            var values = parts.Skip(2).ToArray();

            var result = await Storage.LPushAsync(key, values);

            return result;

        }
    }
}
