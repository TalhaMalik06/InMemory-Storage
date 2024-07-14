using InMemory_Storage.Messages;
using InMemory_Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Commands
{
    public class GetItemCommandHandler : ICommandHandler
    {
        public GetItemCommandHandler(IKeyValueRepository keyValueRepository)
        {
            KeyValueRepository = keyValueRepository ?? throw new ArgumentNullException(nameof(keyValueRepository));
        }

        private IKeyValueRepository KeyValueRepository { get; }
        public bool CanHandle(string commandType)
        {
            return commandType.Equals("GET", StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            {
                var parts = command.Split(' ');
                if (parts.Length < 2)
                {
                    return Task.FromResult(ErrorMessages.InvalidCommandFormatForGet);
                }

                var key = parts[1];
                var value = KeyValueRepository.GetItem(key);

                return Task.FromResult(value);
            }
        }
    }
}
