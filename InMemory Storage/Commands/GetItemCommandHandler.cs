using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Commands
{
    public class GetItemCommandHandler : ICommandHandler
    {
        public bool CanHandle(string commandType)
        {
            return commandType.Equals("GET", StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> Handle(string command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
