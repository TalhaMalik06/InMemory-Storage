using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Commands
{
    public interface ICommandFactory
    {
        public ICommandHandler GetCommandHandler(string commandName);
    }
}
