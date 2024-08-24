using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Repository
{
    public interface IListRepository
    {
        void LPush(string key, params string[] values);
        void RPush(string key, params string[] values);
        string LPop(string key);
        string RPop(string key);
        IEnumerable<string> LRange(string key, int start, int stop);
        int LLen(string key);
        Dictionary<string, List<string>> GetAllData();
        void RestoreData(Dictionary<string, List<string>> data);
    }

}
