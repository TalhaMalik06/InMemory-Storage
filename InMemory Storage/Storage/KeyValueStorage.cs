using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Storage
{
    public class KeyValueStorage : IKeyValueStorage
    {
        private readonly ConcurrentDictionary<string, string> Storage = new();

        public void Set(string key, string value)
        {
            Storage[key] = value;
        }

        public string? Get(string key)
        {
            Storage.TryGetValue(key, out var value);
            return value;
        }

        public void Delete(string key)
        {
            Storage.TryRemove(key, out _);
        }

        public bool ContainsKey(string key)
        {
            return Storage.ContainsKey(key);
        }
    }
}
