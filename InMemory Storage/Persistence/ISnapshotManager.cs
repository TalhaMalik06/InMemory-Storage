using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemory_Storage.Persistence
{
    public interface ISnapshotManager
    {
        void SnapshotCallback(object? state);
        Task CreateSnapshot();
        Task RestoreFromSnapshot();
    }
}
