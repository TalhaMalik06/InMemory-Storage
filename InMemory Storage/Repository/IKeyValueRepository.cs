﻿using InMemory_Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Repository
{
    public interface IKeyValueRepository
    {
        public string Get(string key);
        public void Set(string key, string value, TimeSpan? ttl = null);
        public void Delete(string key);
        public bool ContainsKey(string key);
        Dictionary<string, ValueWithExpiry> GetAllData();
        void RestoreData(Dictionary<string, ValueWithExpiry> data);
    }
}
