using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Repository
{
    public class ListRepository : IListRepository
    {
        private readonly ConcurrentDictionary<string, List<string>> Lists = new();

        public Task<string> LPushAsync(string key, params string[] values)
        {
            var list = Lists.GetOrAdd(key, _ => new List<string>());
            lock (list)
            {
                list.InsertRange(0, values);
            }
            return Task.FromResult("OK");
        }

        public Task<string> RPushAsync(string key, params string[] values)
        {
            var list = Lists.GetOrAdd(key, _ => new List<string>());
            lock (list)
            {
                list.AddRange(values);
            }
            return Task.FromResult("OK");
        }

        public Task<string> LPopAsync(string key)
        {
            if (Lists.TryGetValue(key, out var list))
            {
                lock (list)
                {
                    if (list.Count > 0)
                    {
                        var value = list[0];
                        list.RemoveAt(0);
                        return Task.FromResult(value);
                    }
                }
            }
            return Task.FromResult<string>(null);
        }

        public Task<string> RPopAsync(string key)
        {
            if (Lists.TryGetValue(key, out var list))
            {
                lock (list)
                {
                    if (list.Count > 0)
                    {
                        var value = list[list.Count - 1];
                        list.RemoveAt(list.Count - 1);
                        return Task.FromResult(value);
                    }
                }
            }
            return Task.FromResult<string>(null);
        }

        public Task<IEnumerable<string>> LRangeAsync(string key, int start, int stop)
        {
            if (Lists.TryGetValue(key, out var list))
            {
                lock (list)
                {
                    start = Math.Max(start, 0);
                    stop = Math.Min(stop, list.Count - 1);
                    return Task.FromResult(list.Skip(start).Take(stop - start + 1).AsEnumerable());
                }
            }
            return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());
        }

        public Task<int> LLenAsync(string key)
        {
            if (Lists.TryGetValue(key, out var list))
            {
                lock (list)
                {
                    return Task.FromResult(list.Count);
                }
            }
            return Task.FromResult(0);
        }
    }
}
