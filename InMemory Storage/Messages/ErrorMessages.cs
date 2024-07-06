using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Messages
{
    public static class ErrorMessages
    {
        public const string TcpSettingNull = "TCP server settings or address cannot be null or empty.";
        public const string TcpPortInvalid = "Invalid TCP server port number.";
    }
}
