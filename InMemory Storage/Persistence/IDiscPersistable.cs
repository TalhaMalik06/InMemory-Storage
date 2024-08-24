﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Persistence
{
    public interface IDiscPersistable<T>
    {
        Dictionary<string, T> GetAllData();
        void RestoreData(Dictionary<string, T> data);
    }
}
