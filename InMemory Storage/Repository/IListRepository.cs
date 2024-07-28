using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Repository
{
    public interface IListRepository
    {
        Task<string> LPushAsync(string key, params string[] values);
        Task<string> RPushAsync(string key, params string[] values);
        Task<string> LPopAsync(string key);
        Task<string> RPopAsync(string key);
        Task<IEnumerable<string>> LRangeAsync(string key, int start, int stop);
        Task<int> LLenAsync(string key);
    }

}
