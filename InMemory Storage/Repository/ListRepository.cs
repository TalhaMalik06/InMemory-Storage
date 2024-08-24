using System.Collections.Concurrent;

namespace InMemory_Storage.Repository
{
    public class ListRepository : IListRepository
    {
        private readonly ConcurrentDictionary<string, List<string>> Lists = new();

        public void LPush(string key, params string[] values)
        {
            var list = Lists.GetOrAdd(key, _ => new List<string>());
            lock (list)
            {
                list.InsertRange(0, values);
            }
        }

        public void RPush(string key, params string[] values)
        {
            var list = Lists.GetOrAdd(key, _ => new List<string>());
            lock (list)
            {
                list.AddRange(values);
            }
        }

        public string LPop(string key)
        {
            if (Lists.TryGetValue(key, out var list))
            {
                lock (list)
                {
                    if (list.Count > 0)
                    {
                        var value = list[0];
                        list.RemoveAt(0);
                        return value;
                    }
                }
            }
            return string.Empty;
        }

        public string RPop(string key)
        {
            if (Lists.TryGetValue(key, out var list))
            {
                lock (list)
                {
                    if (list.Count > 0)
                    {
                        var value = list[list.Count - 1];
                        list.RemoveAt(list.Count - 1);
                        return value;
                    }
                }
            }
            return string.Empty;
        }

        public IEnumerable<string> LRange(string key, int start, int stop)
        {
            if (Lists.TryGetValue(key, out var list))
            {
                lock (list)
                {
                    start = Math.Max(start, 0);
                    stop = Math.Min(stop, list.Count - 1);
                    return list.Skip(start).Take(stop - start + 1).AsEnumerable();
                }
            }
            return Array.Empty<string>();
        }

        public int LLen(string key)
        {
            if (Lists.TryGetValue(key, out var list))
            {
                lock (list)
                {
                    return list.Count;
                }
            }
            return 0;
        }

        public Dictionary<string, List<string>> GetAllData()
        {
            return new Dictionary<string, List<string>>(Lists.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList()));
        }

        public void RestoreData(Dictionary<string, List<string>> data)
        {
            Lists.Clear();
            foreach (var kvp in data)
            {
                Lists[kvp.Key] = kvp.Value;
            }
        }
    }
}
