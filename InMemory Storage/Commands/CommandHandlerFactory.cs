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
            return Handlers.FirstOrDefault(h => h.CanHandle(commandName));
        }
    }
}
