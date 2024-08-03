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
        public const string ErrorAcceptingClient = "Error accepting client connection.";
        public const string ErrorHandlingClient = "Error handling client.";
        public const string ClientClosedConnection = "Client connection closed.";
        public const string CommandNameCannotBeNull = "Command name cannot be null or whitespace.";
        public const string NoCommandHandlerFound = "No command handler found for command '{0}'";
        public const string InvalidCommandFormatForSet = "ERROR: Invalid command format. Use: SET <key> <value>";
        public const string FieldCannotBeNullOrEmpty = "Field {0} cannot be null or empty.";
        public const string InvalidCommandFormatForGet = "ERROR: Invalid command format. Use: GET <key>";
        public const string InvalidCommandFormatForDelete = "ERROR: Invalid command format. Use: DELETE <key>";
        public const string InvalidCommandFormatForSetWithExpiry = "ERROR: Invalid command format. Use: SETEX <key> <ttl> <value>";
        public const string InvalidCommandFormatForLpush = "ERROR: Invalid command format. Use: LPUSH <key> <value> [value ...]";

    }
}
