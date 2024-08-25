using InMemory_Storage.Models;

namespace InMemory_Storage.Persistence
{
    public class SnapshotData
    {
        public Dictionary<string, List<string>>? Lists { get; set; }
        public Dictionary<string, ValueWithExpiry>? KeyValuePairs { get; set; }
    }
}
