using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Repository
{
    public class KeyValueRepository : IKeyValueRepository
    {
        private readonly ConcurrentDictionary<string, (string Value, DateTime? Expiry)> Storage = new();

        public void Set(string key, string value, TimeSpan? ttl = null)
        {
            var expiry = ttl.HasValue ? DateTime.UtcNow.Add(ttl.Value) : (DateTime?)null;
            Storage[key] = (value, expiry);
        }

        public string? Get(string key)
        {
            if (Storage.TryGetValue(key, out var entry))
            {
                if (entry.Expiry.HasValue && entry.Expiry.Value <= DateTime.UtcNow)
                {
                    Storage.TryRemove(key, out _);
                    return null;
                }
                return entry.Value;
            }
            return null;
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
