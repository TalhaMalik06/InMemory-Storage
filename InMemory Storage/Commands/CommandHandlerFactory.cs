using InMemory_Storage.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Commands
{
    public class CommandHandlerFactory : ICommandFactory
    {
        public CommandHandlerFactory(IEnumerable<ICommandHandler> commandHandlers)
        {
            Handlers = commandHandlers ?? throw new ArgumentNullException(nameof(commandHandlers));        
        }

        IEnumerable<ICommandHandler> Handlers { get; set; }
        public ICommandHandler GetCommandHandler(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
            {
                throw new ArgumentException(ErrorMessages.CommandNameCannotBeNull, nameof(commandName));
            }

            var handler = Handlers.FirstOrDefault(h => h.CanHandle(commandName));

            if (handler == null)
            {
                throw new InvalidOperationException(string.Format(ErrorMessages.NoCommandHandlerFound, commandName));
            }

            return handler;
        }
    }
}
