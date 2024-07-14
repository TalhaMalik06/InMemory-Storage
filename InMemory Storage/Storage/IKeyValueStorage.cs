using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Storage
{
    public interface IKeyValueStorage
    {
        public string? Get(string key);
        public void Set(string key, string value);
        public void Delete(string key);
        public bool ContainsKey(string key);
    }
}
