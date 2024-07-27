using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Repository
{
    public interface IKeyValueRepository
    {
        string GetItem(string key);
        void SetItem(string key, string value, TimeSpan? ttl = null);
        void DeleteItem(string key);
    }
}
