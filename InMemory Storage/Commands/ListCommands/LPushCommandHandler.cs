﻿using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;

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
                throw new CommandFormatException(ErrorMessages.InvalidCommandFormatForLpush);
            }
            var key = parts[1];
            var values = parts.Skip(2).ToArray();
            await Task.Run(() => Storage.LPush(key, values), cancellationToken);

            return ResponseMessages.SuccessCode;

        }
    }
}
