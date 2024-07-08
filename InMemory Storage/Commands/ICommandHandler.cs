using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Commands
{
    public interface ICommandHandler
    {
        public bool CanHandle(string commandType);
        public Task<string> Handle(string command, CancellationToken cancellationToken);
    }
}
