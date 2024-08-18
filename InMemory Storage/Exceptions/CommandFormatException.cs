using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Exceptions
{
    public class CommandFormatException : Exception
    {
        public CommandFormatException(string message) : base(message) { }

    }
}