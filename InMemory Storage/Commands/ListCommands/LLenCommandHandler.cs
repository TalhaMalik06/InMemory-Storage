﻿using InMemory_Storage.Exceptions;
using InMemory_Storage.Messages;
using InMemory_Storage.Repository;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InMemory_Storage.Commands.ListCommands
{
    public class LLenCommandHandler : ICommandHandler
    {
        public LLenCommandHandler(IListRepository storage)
        {
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        private readonly IListRepository Storage;

        public bool CanHandle(string commandType)
        {
            return commandType.Equals("LLEN", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            var parts = command.Split(' ');

            if (parts.Length != 2)
            {
                throw new CommandFormatException(ErrorMessages.InvalidCommandFormatForLLen);
            }

            var key = parts[1];

            var length = await Storage.LLenAsync(key);

            return length.ToString();
        }
    }
}